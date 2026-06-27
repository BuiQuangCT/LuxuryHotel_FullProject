using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using lxrhotel.API.Models;
using lxrhotel.API.Controllers;

namespace lxrhotel.API.Tests
{
    public class DatPhongControllerTests
    {
        private readonly DbContextOptions<LuxuryHotelContext> _dbOptions;

        public DatPhongControllerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<LuxuryHotelContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private LuxuryHotelContext CreateContext() => new LuxuryHotelContext(_dbOptions);

        #region Test cho chức năng Tạo đơn

        [Fact]
        public async Task TaoDonDatPhong_ShouldCreatePendingBooking_AndReturnOk()
        {
            // Arrange
            var context = CreateContext();
            var controller = new DatPhongController(context);
            var request = new DatPhongController.TaoDonRequest
            {
                MaKh = 1,
                MaPhong = "P101",
                NgayNhan = DateTime.Now.AddDays(1),
                NgayTra = DateTime.Now.AddDays(3),
                TongTien = 2000
            };

            // Act
            var result = await controller.TaoDonDatPhong(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var createdBooking = await context.DatPhongs.FirstOrDefaultAsync();
            createdBooking.Should().NotBeNull();
            createdBooking.TrangThai.Should().Be("Pending");
            createdBooking.MaKh.Should().Be(request.MaKh);
        }

        #endregion

        #region Test cho chức năng Hủy đơn

        [Fact]
        public async Task HuyDon_ShouldCancelPendingOrder_AndReturnOk()
        {
            // Arrange
            var context = CreateContext();
            var booking = new DatPhong { MaDatPhong = 1, TrangThai = "Pending" };
            context.DatPhongs.Add(booking);
            await context.SaveChangesAsync();
            var controller = new DatPhongController(context);

            // Act
            var result = await controller.HuyDon(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var updatedBooking = await context.DatPhongs.FindAsync(1);
            updatedBooking.TrangThai.Should().Be("Đã hủy");
        }

        [Theory]
        [InlineData("Success")]
        [InlineData("Đã hủy")]
        [InlineData("Failed")]
        public async Task HuyDon_ShouldReturnBadRequest_WhenOrderIsNotInPendingState(string initialStatus)
        {
            // Arrange
            var context = CreateContext();
            var booking = new DatPhong { MaDatPhong = 1, TrangThai = initialStatus };
            context.DatPhongs.Add(booking);
            await context.SaveChangesAsync();
            var controller = new DatPhongController(context);

            // Act
            var result = await controller.HuyDon(1);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                   .Which.Value.Should().Be("Không thể hủy đơn hàng ở trạng thái này.");
        }

        #endregion
    }
}