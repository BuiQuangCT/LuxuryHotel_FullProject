using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using Moq;
using lxrhotel.API.Models;
using lxrhotel.API.Controllers;
using lxrhotel.API.Services;

namespace lxrhotel.API.Tests
{
    public class KhachHangControllerTests
    {
        private readonly DbContextOptions<LuxuryHotelContext> _dbOptions;
        private readonly IConfiguration _configuration;
        private readonly Mock<IEmailService> _mockEmailService;

        public KhachHangControllerTests()
        {
           
            _dbOptions = new DbContextOptionsBuilder<LuxuryHotelContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "LXR_Hotel_Super_Secret_Key_At_Least_32_Chars_Long_2026!!!"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

           
            _mockEmailService = new Mock<IEmailService>();
        }

        private LuxuryHotelContext CreateContext() => new LuxuryHotelContext(_dbOptions);

        #region Test cho chức năng Đăng nhập

        [Fact]
        public async Task DangNhap_ShouldReturnToken_WhenCredentialsAreCorrect()
        {
            // Arrange
            var context = CreateContext();
            var email = "test@example.com";
            var password = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new KhachHang
            {
                MaKh = 1,
                HoTen = "Test User",
                Email = email,
                MatKhau = hashedPassword,
                TrangThai = "active",
                VaiTro = "KhachHang"
            };
            context.KhachHangs.Add(user);
            await context.SaveChangesAsync();

            var controller = new KhachHangController(context, _configuration, _mockEmailService.Object);

            // Act
            var result = await controller.DangNhap(email, password);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().NotBeNull();

            var data = okResult.Value;
            var token = data.GetType().GetProperty("token")?.GetValue(data, null) as string;
            var vaiTro = data.GetType().GetProperty("vaiTro")?.GetValue(data, null) as string;

            token.Should().NotBeNullOrEmpty();
            vaiTro.Should().Be("KhachHang");
        }

        [Fact]
        public async Task DangNhap_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var context = CreateContext();
            var controller = new KhachHangController(context, _configuration, _mockEmailService.Object);

            // Act
            var result = await controller.DangNhap("nonexistent@user.com", "password");

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                   .Which.Value.Should().Be("Tài khoản không tồn tại!");
        }

        [Fact]
        public async Task DangNhap_ShouldReturnBadRequest_WhenPasswordIsIncorrect()
        {
            // Arrange
            var context = CreateContext();
            var email = "test@example.com";
            var password = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new KhachHang { MaKh = 1, HoTen = "Test User", Email = email, MatKhau = hashedPassword, TrangThai = "active" };
            context.KhachHangs.Add(user);
            await context.SaveChangesAsync();

            var controller = new KhachHangController(context, _configuration, _mockEmailService.Object);

            // Act
            var result = await controller.DangNhap(email, "wrongpassword");

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                   .Which.Value.Should().Be("Sai mật khẩu!");
        }

        [Fact]
        public async Task DangNhap_ShouldReturnUnauthorized_WhenAccountIsLocked()
        {
            // Arrange
            var context = CreateContext();
            var email = "locked@example.com";
            var password = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new KhachHang { MaKh = 1, HoTen = "Test User", Email = email, MatKhau = hashedPassword, TrangThai = "locked" };
            context.KhachHangs.Add(user);
            await context.SaveChangesAsync();

            var controller = new KhachHangController(context, _configuration, _mockEmailService.Object);

            // Act
            var result = await controller.DangNhap(email, password);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>()
                   .Which.Value.Should().Be("Tài khoản của bạn đã bị khóa!");
        }

        #endregion

        #region Test cho chức năng Quên mật khẩu

        [Fact]
        public async Task QuenMatKhau_ShouldUpdatePasswordAndSendEmail_WhenEmailExists()
        {
            // Arrange
            var context = CreateContext();
            var email = "forgot@example.com";
            var oldPassword = "old_password";
            var oldHash = BCrypt.Net.BCrypt.HashPassword(oldPassword);
            var user = new KhachHang { MaKh = 1, Email = email, MatKhau = oldHash, HoTen = "Test User" };
            context.KhachHangs.Add(user);
            await context.SaveChangesAsync();

            var controller = new KhachHangController(context, _configuration, _mockEmailService.Object);

            // Act
            var request = new KhachHangController.QuenMatKhauRequest { Email = email };
            var result = await controller.QuenMatKhau(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var updatedUser = await context.KhachHangs.FindAsync(1);
            updatedUser.MatKhau.Should().NotBe(oldHash);

            // Verify that the email service was called correctly
            _mockEmailService.Verify(s => s.SendNewPasswordAsync(
                email, "Test User", It.Is<string>(p => p.StartsWith("LXR"))
            ), Times.Once);
        }
        #endregion
    }
}