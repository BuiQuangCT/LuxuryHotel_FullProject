using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using lxrhotel.API.Models;
using lxrhotel.API.Controllers;

namespace lxrhotel.API.Tests
{
    public class AdminControllerTests
    {
        private readonly DbContextOptions<LuxuryHotelContext> _dbOptions;

        // Hàm khởi tạo này sẽ chạy trước mỗi test, đảm bảo mỗi test có một DB "sạch"
        public AdminControllerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<LuxuryHotelContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Sử dụng DB trong bộ nhớ với tên ngẫu nhiên
                .Options;
        }

        // Hàm helper để tạo context cho mỗi test
        private LuxuryHotelContext CreateContext() => new LuxuryHotelContext(_dbOptions);

        #region Test cho chức năng Cập nhật trạng thái phòng

        [Fact]
        public async Task CapNhatTrangThaiPhong_ShouldReturnOk_WhenRoomExistsAndNotOccupied()
        {
            // Arrange - Chuẩn bị dữ liệu
            var context = CreateContext();
            var phong = new Phong { MaPhong = "P101", TrangThai = "Trống", LoaiPhong = "Standard", MaKs = "KS01" };
            context.Phongs.Add(phong);
            await context.SaveChangesAsync();

            var controller = new AdminController(context);
            var trangThaiMoi = "Bảo trì";

            // Act - Thực thi hành động
            var result = await controller.CapNhatTrangThaiPhong(phong.MaPhong, trangThaiMoi);

            // Assert - Kiểm tra kết quả
            result.Should().BeOfType<OkObjectResult>();
            var updatedPhong = await context.Phongs.FindAsync(phong.MaPhong);
            updatedPhong.TrangThai.Should().Be(trangThaiMoi);
        }

        [Fact]
        public async Task CapNhatTrangThaiPhong_ShouldReturnNotFound_WhenRoomDoesNotExist()
        {
            // Arrange
            var context = CreateContext();
            var controller = new AdminController(context);

            // Act
            var result = await controller.CapNhatTrangThaiPhong("P_NON_EXIST", "Trống");

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CapNhatTrangThaiPhong_ShouldReturnBadRequest_WhenRoomIsOccupied()
        {
            // Arrange
            var context = CreateContext();
            var phong = new Phong { MaPhong = "P101", TrangThai = "Trống", LoaiPhong = "Standard", MaKs = "KS01" };
            var datPhong = new DatPhong
            {
                MaDatPhong = 1,
                MaPhong = "P101",
                TrangThai = "Success",
                NgayNhan = DateTime.Now.AddDays(-1),
                NgayTra = DateTime.Now.AddDays(2)
            };
            context.Phongs.Add(phong);
            context.DatPhongs.Add(datPhong);
            await context.SaveChangesAsync();

            var controller = new AdminController(context);

            // Act
            var result = await controller.CapNhatTrangThaiPhong(phong.MaPhong, "Bảo trì");

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                   .Which.Value.Should().Be("Không thể thay đổi trạng thái. Phòng này hiện đang có khách ở.");
        }

        #endregion

        #region Test cho chức năng Thống kê doanh thu

        [Fact]
        public async Task ThongKeDoanhThu_ShouldCalculateCorrectly_ForSuccessfulBookingsInPeriod()
        {
            // Arrange
            var context = CreateContext();
            var thang = 5;
            var nam = 2024;

            var phong1 = new Phong { MaPhong = "P101", LoaiPhong = "Standard", MaKs = "KS01", TrangThai = "Trống" };
            var phong2 = new Phong { MaPhong = "P102", LoaiPhong = "Standard", MaKs = "KS01", TrangThai = "Trống" };
            var phong3 = new Phong { MaPhong = "P103", LoaiPhong = "Standard", MaKs = "KS01", TrangThai = "Trống" };
            var phong4 = new Phong { MaPhong = "P104", LoaiPhong = "Standard", MaKs = "KS01", TrangThai = "Trống" };

            // Đơn thành công trong kỳ
            var dp1 = new DatPhong { MaDatPhong = 1, MaPhong = "P101", TrangThai = "Success", TongTien = 1000 };
            var dc1 = new DatCoc { MaDatCoc = 1, MaDatPhong = 1, NgayDatCoc = new DateTime(nam, thang, 10) };

            var dp2 = new DatPhong { MaDatPhong = 2, MaPhong = "P102", TrangThai = "Success", TongTien = 2500 };
            var dc2 = new DatCoc { MaDatCoc = 2, MaDatPhong = 2, NgayDatCoc = new DateTime(nam, thang, 15) };

            // Đơn đang chờ trong kỳ
            var dp3 = new DatPhong { MaDatPhong = 3, MaPhong = "P103", TrangThai = "Pending", TongTien = 500 };
            var dc3 = new DatCoc { MaDatCoc = 3, MaDatPhong = 3, NgayDatCoc = new DateTime(nam, thang, 20) };

            // Đơn thành công ngoài kỳ
            var dp4 = new DatPhong { MaDatPhong = 4, MaPhong = "P104", TrangThai = "Success", TongTien = 9999 };
            var dc4 = new DatCoc { MaDatCoc = 4, MaDatPhong = 4, NgayDatCoc = new DateTime(nam, thang + 1, 1) };

            context.Phongs.AddRange(phong1, phong2, phong3, phong4);
            context.DatPhongs.AddRange(dp1, dp2, dp3, dp4);
            context.DatCocs.AddRange(dc1, dc2, dc3, dc4);
            await context.SaveChangesAsync();

            var controller = new AdminController(context);

            // Act
            var result = await controller.ThongKeDoanhThu(thang, nam);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var data = (result as OkObjectResult).Value;

            // Sử dụng dynamic để truy cập thuộc tính của đối tượng anonymous
            ((decimal)((dynamic)data).tongDoanhThu).Should().Be(3500); // 1000 + 2500
            ((int)((dynamic)data).soLuongDonHang).Should().Be(2);
        }

        #endregion

        #region Test cho chức năng Cập nhật trạng thái đơn

        [Fact]
        public async Task CapNhatTrangThaiDon_ShouldChangeStatusToSuccess_AndReturnOk()
        {
            // Arrange
            var context = CreateContext();
            var phong = new Phong { MaPhong = "P101", LoaiPhong = "Standard", MaKs = "KS01", TrangThai = "Trống" };
            var don = new DatPhong { MaDatPhong = 1, MaPhong = "P101", TrangThai = "Pending", TongTien = 1200 };
            context.Phongs.Add(phong);
            context.DatPhongs.Add(don);
            await context.SaveChangesAsync();

            var controller = new AdminController(context);

            // Act
            var result = await controller.CapNhatTrangThaiDon(1, "Success");

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var updatedDon = await context.DatPhongs.FindAsync(1);
            updatedDon.TrangThai.Should().Be("Success");
        }

        [Fact]
        public async Task CapNhatTrangThaiDon_ShouldCreateDatCocRecord_WhenApprovingPendingOrder()
        {
            // Arrange
            var context = CreateContext();
            var phong = new Phong { MaPhong = "P101", LoaiPhong = "Standard", MaKs = "KS01", TrangThai = "Trống" };
            var don = new DatPhong { MaDatPhong = 1, MaPhong = "P101", TrangThai = "Pending", TongTien = 1200 };
            context.Phongs.Add(phong);
            context.DatPhongs.Add(don);
            await context.SaveChangesAsync();

            var controller = new AdminController(context);

            // Act
            await controller.CapNhatTrangThaiDon(1, "Success");

            // Assert
            var datCocMoi = await context.DatCocs.FirstOrDefaultAsync(dc => dc.MaDatPhong == 1);
            datCocMoi.Should().NotBeNull();
            datCocMoi.SoTienCoc.Should().Be(1200);
            datCocMoi.TrangThai.Should().Be("Đã thanh toán");
        }

        [Fact]
        public async Task CapNhatTrangThaiDon_ShouldChangeStatusToCancelled_AndReturnOk()
        {
            // Arrange
            var context = CreateContext();
            var phong = new Phong { MaPhong = "P101", LoaiPhong = "Standard", MaKs = "KS01", TrangThai = "Trống" };
            var don = new DatPhong { MaDatPhong = 1, MaPhong = "P101", TrangThai = "Pending" };
            context.Phongs.Add(phong);
            context.DatPhongs.Add(don);
            await context.SaveChangesAsync();

            var controller = new AdminController(context);

            // Act
            var result = await controller.CapNhatTrangThaiDon(1, "Đã hủy");

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var updatedDon = await context.DatPhongs.FindAsync(1);
            updatedDon.TrangThai.Should().Be("Đã hủy");
        }

        #endregion

        #region Test cho chức năng Khóa/Mở khóa tài khoản

        [Theory]
        [InlineData("active", "locked")]
        [InlineData("locked", "active")]
        public async Task KhoaTaiKhoan_ShouldToggleUserStatus(string initialStatus, string expectedStatus)
        {
            // Arrange
            var context = CreateContext();
            var khachHang = new KhachHang { MaKh = 1, TrangThai = initialStatus, Email = "test@example.com", HoTen = "Test User", MatKhau = "password" };
            context.KhachHangs.Add(khachHang);
            await context.SaveChangesAsync();

            var controller = new AdminController(context);

            // Act
            var result = await controller.KhoaTaiKhoan(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var updatedKhachHang = await context.KhachHangs.FindAsync(1);
            updatedKhachHang.TrangThai.Should().Be(expectedStatus);
        }

        #endregion
    }
}