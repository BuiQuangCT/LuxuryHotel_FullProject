using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using lxrhotel.API.Models;
using lxrhotel.API.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace lxrhotel.API.Tests
{
    public class PhongControllerTests
    {
        private readonly DbContextOptions<LuxuryHotelContext> _dbOptions;

        public PhongControllerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<LuxuryHotelContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private LuxuryHotelContext CreateContext() => new LuxuryHotelContext(_dbOptions);

        #region Test cho chức năng Tìm kiếm phòng

        [Fact]
        public async Task TimKiemPhong_ShouldReturnAvailableRooms_WhenCriteriaMatch()
        {
            // Arrange
            var context = CreateContext();
            var khachSan = new KhachSan { MaKs = "KS01", TenKs = "Luxury SG", DiaDiem = "TP.HCM" };
            var phong1 = new Phong { MaPhong = "P101", MaKs = "KS01", SucChua = 2, MaKsNavigation = khachSan };
            var phong2 = new Phong { MaPhong = "P102", MaKs = "KS01", SucChua = 4, MaKsNavigation = khachSan };
            context.KhachSans.Add(khachSan);
            context.Phongs.AddRange(phong1, phong2);
            await context.SaveChangesAsync();

            var controller = new PhongController(context);

            // Act
            var ngayNhan = new DateTime(2024, 12, 20);
            var ngayTra = new DateTime(2024, 12, 22);
            var result = await controller.TimKiemPhong("TP.HCM", ngayNhan, ngayTra, 2);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var rooms = (okResult.Value as IEnumerable<object>).ToList();
            rooms.Should().HaveCount(2);
        }

        [Fact]
        public async Task TimKiemPhong_ShouldExcludeBookedRooms()
        {
            // Arrange
            var context = CreateContext();
            var khachSan = new KhachSan { MaKs = "KS01", TenKs = "Luxury SG", DiaDiem = "TP.HCM" };
            var phong1 = new Phong { MaPhong = "P101", MaKs = "KS01", SucChua = 2, MaKsNavigation = khachSan }; // Available
            var phong2 = new Phong { MaPhong = "P102", MaKs = "KS01", SucChua = 2, MaKsNavigation = khachSan }; // Booked
            var datPhong = new DatPhong
            {
                MaDatPhong = 1, MaPhong = "P102", TrangThai = "Success",
                NgayNhan = new DateTime(2024, 12, 21), NgayTra = new DateTime(2024, 12, 23) // Overlaps
            };
            context.KhachSans.Add(khachSan);
            context.Phongs.AddRange(phong1, phong2);
            context.DatPhongs.Add(datPhong);
            await context.SaveChangesAsync();

            var controller = new PhongController(context);

            // Act
            var ngayNhan = new DateTime(2024, 12, 20);
            var ngayTra = new DateTime(2024, 12, 22);
            var result = await controller.TimKiemPhong("TP.HCM", ngayNhan, ngayTra, 2);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var rooms = (okResult.Value as IEnumerable<dynamic>).ToList();
            rooms.Should().HaveCount(1);
            rooms.First().MaPhong.Should().Be("P101");
        }

        [Fact]
        public async Task TimKiemPhong_ShouldReturnBadRequest_WhenDatesAreInvalid()
        {
            // Arrange
            var context = CreateContext();
            var controller = new PhongController(context);

            // Act
            var ngayNhan = new DateTime(2024, 12, 22);
            var ngayTra = new DateTime(2024, 12, 20);
            var result = await controller.TimKiemPhong("TP.HCM", ngayNhan, ngayTra, 2);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Ngày nhận phải trước ngày trả.");
        }

        #endregion

        #region Test cho chức năng Lấy chi tiết phòng

        [Fact]
        public async Task GetChiTietPhong_ShouldReturnRoomDetails_WhenRoomExists()
        {
            // Arrange
            var context = CreateContext();
            var khachSan = new KhachSan { MaKs = "KS01", TenKs = "Luxury SG", DiaDiem = "TP.HCM", DiaChi = "123 Le Loi" };
            var phong = new Phong { MaPhong = "P101", LoaiPhong = "Deluxe", MaKs = "KS01", MaKsNavigation = khachSan };
            context.KhachSans.Add(khachSan);
            context.Phongs.Add(phong);
            await context.SaveChangesAsync();

            var controller = new PhongController(context);

            // Act
            var result = await controller.GetChiTietPhong("P101");

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var roomDetails = okResult.Value as dynamic;
            ((string)roomDetails.MaPhong).Should().Be("P101");
            ((string)roomDetails.LoaiPhong).Should().Be("Deluxe");
            ((string)roomDetails.KhachSan.TenKS).Should().Be("Luxury SG");
        }

        [Fact]
        public async Task GetChiTietPhong_ShouldReturnNotFound_WhenRoomDoesNotExist()
        {
            // Arrange
            var context = CreateContext();
            var controller = new PhongController(context);

            // Act
            var result = await controller.GetChiTietPhong("P_NON_EXIST");

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        #endregion
    }
}