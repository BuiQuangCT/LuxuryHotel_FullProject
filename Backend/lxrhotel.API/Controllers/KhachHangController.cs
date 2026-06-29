﻿using BCrypt.Net;
using lxrhotel.API.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using lxrhotel.API.Services;

namespace lxrhotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly LuxuryHotelContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public KhachHangController(LuxuryHotelContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

       
        [HttpPost("dang-ky")]
        public async Task<IActionResult> DangKy([FromBody] KhachHang request)
        {
          
            var checkTonTai = await _context.KhachHangs
                .AnyAsync(x => x.Email == request.Email || x.SoDienThoai == request.SoDienThoai);

            if (checkTonTai)
                return BadRequest("Email hoặc Số điện thoại đã được sử dụng!");

           
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.MatKhau);

            var newKhachHang = new KhachHang
            {
                HoTen = request.HoTen,
                Email = request.Email,
                SoDienThoai = request.SoDienThoai,
                Cmnd = request.Cmnd,
                MatKhau = passwordHash, 
                TrangThai = "active"
            };

            _context.KhachHangs.Add(newKhachHang);
            await _context.SaveChangesAsync();

            return Ok(new { thongBao = "Đăng ký tài khoản thành công!" });
        }

        
        [HttpPost("dang-nhap")]
        public async Task<IActionResult> DangNhap(string email, string matKhauGoc)
        {
            
            var user = await _context.KhachHangs.SingleOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return NotFound("Tài khoản không tồn tại!");

            if (user.TrangThai == "locked")
                return Unauthorized("Tài khoản của bạn đã bị khóa!");

           
            bool checkPass = BCrypt.Net.BCrypt.Verify(matKhauGoc, user.MatKhau);

            if (!checkPass)
                return BadRequest("Sai mật khẩu!");

          
        

          
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.MaKh.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            
            if (user.VaiTro == "Admin")
            {
              
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                
                claims.Add(new Claim(ClaimTypes.Role, "KhachHang"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), 
                Expires = DateTime.UtcNow.AddHours(2), 
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                thongBao = "Đăng nhập thành công!",
                token = jwtString, 
                maKh = user.MaKh,    
                vaiTro = user.VaiTro 
            });


        }
      
        public class CapNhatProfileRequest
        {
            public string HoTen { get; set; } = null!;
            public string SoDienThoai { get; set; } = null!;
            public string Cmnd { get; set; } = null!;
        }

      
        
      
        [HttpGet("thong-tin")]
        [Authorize]
        public async Task<IActionResult> GetThongTinCaNhan()
        {
            
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized("Token không hợp lệ.");

           
            var user = await _context.KhachHangs
                .Where(k => k.MaKh == userId)
                .Select(k => new { k.Email, k.HoTen, k.SoDienThoai, k.Cmnd })
                .FirstOrDefaultAsync();

            if (user == null) return NotFound("Không tìm thấy dữ liệu người dùng.");

            return Ok(user);
        }

      
       
      
        [HttpPut("cap-nhat")]
        [Authorize]
        public async Task<IActionResult> CapNhatThongTin([FromBody] CapNhatProfileRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            var user = await _context.KhachHangs.FindAsync(userId);
            if (user == null) return NotFound();

          
            user.HoTen = request.HoTen;
            user.SoDienThoai = request.SoDienThoai;
            user.Cmnd = request.Cmnd;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật hồ sơ thành công!" });
        }

        
        public class QuenMatKhauRequest
        {
            public string Email { get; set; } = null!;
        }

        
        [HttpPost("quen-mat-khau")]
        public async Task<IActionResult> QuenMatKhau([FromBody] QuenMatKhauRequest request)
        {
            var email = request.Email;
            var user = await _context.KhachHangs.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null) return NotFound("Email không tồn tại trong hệ thống!");

         
            string newPassword = "LXR" + new Random().Next(100000, 999999).ToString();

           
            try
            {
                await _emailService.SendNewPasswordAsync(user.Email, user.HoTen, newPassword);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Không thể gửi email cấp lại mật khẩu vào lúc này. Vui lòng thử lại sau.");
            }

            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Một mật khẩu mới đã được gửi đến email của bạn. Vui lòng kiểm tra hộp thư." });
        }
    }
}