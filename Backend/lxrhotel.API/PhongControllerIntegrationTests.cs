using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lxrhotel.API.Tests.IntegrationTests
{
    // 'Program' class được dùng như một "marker" để WebApplicationFactory tìm thấy điểm khởi đầu của API.
    // Đây là một pattern chuẩn cho kiểm thử tích hợp trong ASP.NET Core.
    public class PhongControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PhongControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Tạo một HttpClient để gửi request đến server test.
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task TimKiemPhong_IntegrationTest_ShouldReturnAvailableRooms()
        {
            // Arrange
            // Trong một bài test tích hợp thực tế, bạn có thể "seed" (gieo) dữ liệu vào DB test ở đây.
            // WebApplicationFactory có thể được cấu hình để sử dụng một DB test riêng biệt.
            // Chúng ta sẽ test với một kịch bản tìm kiếm cụ thể.
            var diaDiem = "TP.HCM"; // Sử dụng một địa điểm có trong CSDL của bạn
            var ngayNhan = "2025-01-10";
            var ngayTra = "2025-01-12";
            var soNguoi = 2;

            var requestUri = $"/api/Phong/tim-kiem?diaDiem={diaDiem}&ngayNhan={ngayNhan}&ngayTra={ngayTra}&soNguoi={soNguoi}";

            // Act
            // Gửi một request GET thật sự đến endpoint tìm kiếm.
            var response = await _client.GetAsync(requestUri);

            // Assert
            // 1. Kiểm tra xem request HTTP có thành công không (status code 2xx).
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");

            // 2. Deserialized nội dung response và kiểm tra dữ liệu.
            var rooms = await response.Content.ReadFromJsonAsync<List<object>>();
            rooms.Should().NotBeNull();
        }
    }
}