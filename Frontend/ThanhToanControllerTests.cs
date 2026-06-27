using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using FluentAssertions;
using Moq;
using lxrhotel.API.Models;
using lxrhotel.API.Controllers;
using lxrhotel.API.Services;
using System.Collections.Generic;
using System.Linq;

namespace lxrhotel.API.Tests
{
    public class ThanhToanControllerTests
    {
        private readonly DbContextOptions<LuxuryHotelContext> _dbOptions;
        private readonly Mock<IEmailService> _mockEmailService;

        public ThanhToanControllerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<LuxuryHotelContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _mockEmailService = new Mock<IEmailService>();
        }

        private LuxuryHotelContext CreateContext() => new LuxuryHotelContext(_dbOptions);

        private HttpContext CreateMockHttpContext(Dictionary<string, string> queryParams)
        {
            var httpContext = new DefaultHttpContext();
            var query = new QueryCollection(queryParams.ToDictionary(k => k.Key, v => new StringValues(v.Value)));
            httpContext.Request.Query = query;
            return httpContext;
        }

        #region Test cho chức năng VNPay IPN

        [Fact]
        public async Task VNPayIPN_ShouldConfirmOrderAndSendEmail_WhenPaymentIsSuccessful()
        {
            // Arrange
            var context = CreateContext();
            var donDatPhong = new DatPhong { MaDatPhong = 12345, TrangThai = "Pending", TongTien = 500000, MaKh = 1 };
            var khachHang = new KhachHang { MaKh = 1, Email = "test@test.com", HoTen = "Test User" };
            context.DatPhongs.Add(donDatPhong);
            context.KhachHangs.Add(khachHang);
            await context.SaveChangesAsync();

            var controller = new ThanhToanController(context, _mockEmailService.Object);

            // Mock VNPay response
            var vnpay = new VnPayLibrary();
            vnpay.AddResponseData("vnp_ResponseCode", "00"); // Success
            vnpay.AddResponseData("vnp_TxnRef", "12345");
            string vnp_HashSecret = "Y1UDIWP635I8SD7R7SI43AIE591F5ZUM";
            string secureHash = vnpay.CreateRequestUrl("", vnp_HashSecret).Split('?')[1].Split('=')[1];
            vnpay.AddResponseData("vnp_SecureHash", secureHash);

            controller.ControllerContext.HttpContext = CreateMockHttpContext(vnpay.GetResponseData());

            // Act
            var result = await controller.VNPayIPN();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((dynamic)okResult.Value).RspCode.Should().Be("00");

            var updatedOrder = await context.DatPhongs.FindAsync(12345);
            updatedOrder.TrangThai.Should().Be("Success");

            var datCocRecord = await context.DatCocs.FirstOrDefaultAsync(dc => dc.MaDatPhong == 12345);
            datCocRecord.Should().NotBeNull();

            _mockEmailService.Verify(s => s.SendBookingEmailAsync("test@test.com", "Test User", 12345, 500000), Times.Once);
        }

        [Fact]
        public async Task VNPayIPN_ShouldFailOrder_WhenPaymentFails()
        {
            // Arrange
            var context = CreateContext();
            var donDatPhong = new DatPhong { MaDatPhong = 54321, TrangThai = "Pending" };
            context.DatPhongs.Add(donDatPhong);
            await context.SaveChangesAsync();

            var controller = new ThanhToanController(context, _mockEmailService.Object);

            // Mock VNPay response
            var vnpay = new VnPayLibrary();
            vnpay.AddResponseData("vnp_ResponseCode", "24"); // Failure
            vnpay.AddResponseData("vnp_TxnRef", "54321");
            string vnp_HashSecret = "Y1UDIWP635I8SD7R7SI43AIE591F5ZUM";
            string secureHash = vnpay.CreateRequestUrl("", vnp_HashSecret).Split('?')[1].Split('=')[1];
            vnpay.AddResponseData("vnp_SecureHash", secureHash);

            controller.ControllerContext.HttpContext = CreateMockHttpContext(vnpay.GetResponseData());

            // Act
            await controller.VNPayIPN();

            // Assert
            var updatedOrder = await context.DatPhongs.FindAsync(54321);
            updatedOrder.TrangThai.Should().Be("Failed");
        }

        [Fact]
        public async Task VNPayIPN_ShouldReturnInvalidSignature_WhenHashIsIncorrect()
        {
            // Arrange
            var context = CreateContext();
            var controller = new ThanhToanController(context, _mockEmailService.Object);
            var queryParams = new Dictionary<string, string>
            {
                { "vnp_TxnRef", "111" },
                { "vnp_ResponseCode", "00" },
                { "vnp_SecureHash", "invalid_hash" }
            };
            controller.ControllerContext.HttpContext = CreateMockHttpContext(queryParams);

            // Act
            var result = await controller.VNPayIPN();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((dynamic)okResult.Value).RspCode.Should().Be("97");
            ((dynamic)okResult.Value).Message.Should().Be("Invalid signature");
        }

        #endregion
    }
}