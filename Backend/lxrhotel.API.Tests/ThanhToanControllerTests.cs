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
using System;

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
            var donDatPhong = new DatPhong 
            { 
                MaDatPhong = 12345, 
                TrangThai = "Pending", 
                TongTien = 500000, 
                MaKh = 1,
                MaPhong = "P001",
                NgayNhan = DateTime.Now,
                NgayTra = DateTime.Now.AddDays(2),
                SoNguoi = 2
            };
            var khachHang = new KhachHang 
            { 
                MaKh = 1, 
                Email = "test@test.com", 
                HoTen = "Test User",
                MatKhau = "password"
            };
            var phong = new Phong { MaPhong = "P001", LoaiPhong = "Standard", Gia = 250000, MaKs = "1", TienNghi = "Room description", TrangThai = "Available" };
            context.Phongs.Add(phong);
            context.DatPhongs.Add(donDatPhong);
            context.KhachHangs.Add(khachHang);
            await context.SaveChangesAsync();

            var controller = new ThanhToanController(context, _mockEmailService.Object);

            // Mock VNPay response
            var queryParams = new Dictionary<string, string>
            {
                {"vnp_ResponseCode", "00"},
                {"vnp_TxnRef", "12345"}
            };
            string vnp_HashSecret = "Y1UDIWP635I8SD7R7SI43AIE591F5ZUM";
            
            var vnpay = new VnPayLibrary();
            foreach(var (key, value) in queryParams)
            {
                vnpay.AddResponseData(key, value);
            }
            // Use a stable method to create the hash
            var sortedParams = queryParams.OrderBy(p => p.Key, new VnPayCompare());
            var hashData = string.Join("&", sortedParams.Select(p => $"{p.Key}={p.Value}"));
            // This is not how VNPay calculates hash, but for the test this is enough to have a predictable hash.
            // The controller logic uses ValidateSignature which is what we are testing, not the hash creation.
            // In a real scenario, you'd want to replicate VNPay's hash logic if you were testing the hash creation itself.
            string secureHash = Guid.NewGuid().ToString(); // A mock hash
            vnpay.AddResponseData("vnp_SecureHash", secureHash);
            queryParams["vnp_SecureHash"] = secureHash;


            var mockVnPayLibrary = new Mock<VnPayLibrary>();
            var httpContext = CreateMockHttpContext(queryParams);
            // Since we can't easily mock the VnPayLibrary used inside the controller, 
            // we will have to rely on the fact that with the correct data, it will work.
            // The alternative is to refactor the controller to inject IVnPayLibrary.
            // For now, let's assume checkSignature will be true with a valid-looking setup.
            // Let's create a valid hash
            var vnpayLibForHash = new VnPayLibrary();
            vnpayLibForHash.AddResponseData("vnp_ResponseCode", "00");
            vnpayLibForHash.AddResponseData("vnp_TxnRef", "12345");
            secureHash = vnpayLibForHash.CreateRequestUrl("", vnp_HashSecret).Split('?')[1].Split('&').First(s => s.StartsWith("vnp_SecureHash")).Split('=')[1];
            queryParams["vnp_SecureHash"] = secureHash;


            controller.ControllerContext.HttpContext = CreateMockHttpContext(queryParams);

            // Act
            var result = await controller.VNPayIPN();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().NotBeNull();
            var data = okResult.Value;
            var rspCode = data.GetType().GetProperty("RspCode")?.GetValue(data, null) as string;
            rspCode.Should().Be("00");

            var updatedOrder = await context.DatPhongs.FindAsync(12345);
            updatedOrder.Should().NotBeNull();
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
            var donDatPhong = new DatPhong 
            { 
                MaDatPhong = 54321, 
                TrangThai = "Pending",
                MaKh = 1,
                MaPhong = "P002",
                NgayNhan = DateTime.Now,
                NgayTra = DateTime.Now.AddDays(2),
                SoNguoi = 2,
                TongTien = 100000
            };
            var phong = new Phong { MaPhong = "P002", LoaiPhong = "Standard", Gia = 250000, MaKs = "1", TienNghi = "Room description", TrangThai = "Available" };
            var khachHang = new KhachHang { MaKh = 1, Email = "fail@test.com", HoTen = "Fail User", MatKhau = "password" };
            context.KhachHangs.Add(khachHang);
            context.Phongs.Add(phong);
            context.DatPhongs.Add(donDatPhong);
            await context.SaveChangesAsync();

            var controller = new ThanhToanController(context, _mockEmailService.Object);

            // Mock VNPay response
            var queryParams = new Dictionary<string, string>
            {
                {"vnp_ResponseCode", "24"},
                {"vnp_TxnRef", "54321"}
            };
            string vnp_HashSecret = "Y1UDIWP635I8SD7R7SI43AIE591F5ZUM";
            
            var vnpay = new VnPayLibrary();
            foreach(var (key, value) in queryParams)
            {
                vnpay.AddResponseData(key, value);
            }
            string secureHash = vnpay.CreateRequestUrl("", vnp_HashSecret).Split('?')[1].Split('&').First(s => s.StartsWith("vnp_SecureHash")).Split('=')[1];
            queryParams.Add("vnp_SecureHash", secureHash);

            controller.ControllerContext.HttpContext = CreateMockHttpContext(queryParams);

            // Act
            await controller.VNPayIPN();

            // Assert
            var updatedOrder = await context.DatPhongs.FindAsync(54321);
            updatedOrder.Should().NotBeNull();
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
            okResult.Value.Should().NotBeNull();
            var data = okResult.Value;
            var rspCode = data.GetType().GetProperty("RspCode")?.GetValue(data, null) as string;
            var message = data.GetType().GetProperty("Message")?.GetValue(data, null) as string;

            rspCode.Should().Be("97");
            message.Should().Be("Invalid signature");
        }

        #endregion
    }
}
