using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lxrhotel.API.Tests.IntegrationTests
{
    
    public class PhongControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PhongControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task TimKiemPhong_IntegrationTest_ShouldReturnAvailableRooms()
        {
            
            var diaDiem = "TP.HCM"; 
            var ngayNhan = "2025-01-10";
            var ngayTra = "2025-01-12";
            var soNguoi = 2;

            var requestUri = $"/api/Phong/tim-kiem?diaDiem={diaDiem}&ngayNhan={ngayNhan}&ngayTra={ngayTra}&soNguoi={soNguoi}";

            var response = await _client.GetAsync(requestUri);

            
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");

            
            var rooms = await response.Content.ReadFromJsonAsync<List<object>>();
            rooms.Should().NotBeNull();
        }
    }
}