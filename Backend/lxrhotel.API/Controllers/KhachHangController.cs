using BCrypt.Net;
using lxrhotel.API.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lxrhotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly LuxuryHotelContext _context;

        public KhachHangController(LuxuryHotelContext context)
        {
            _context = context;
        }

        // 1. API Đăng ký tài khoản
        [HttpPost("dang-ky")]
        public async Task<IActionResult> DangKy([FromBody] KhachHang request)
        {
            // Kiểm tra email hoặc SĐT đã tồn tại chưa
            var checkTonTai = await _context.KhachHangs
                .AnyAsync(x => x.Email == request.Email || x.SoDienThoai == request.SoDienThoai);

            if (checkTonTai)
                return BadRequest("Email hoặc Số điện thoại đã được sử dụng!");

            // MÃ HÓA MẬT KHẨU: 
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.MatKhau);

            var newKhachHang = new KhachHang
            {
                HoTen = request.HoTen,
                Email = request.Email,
                SoDienThoai = request.SoDienThoai,
                Cmnd = request.Cmnd,
                MatKhau = passwordHash, // Lưu mật khẩu đã băm (Hash)
                TrangThai = "active"
            };

            _context.KhachHangs.Add(newKhachHang);
            await _context.SaveChangesAsync();

            return Ok(new { thongBao = "Đăng ký tài khoản thành công!" });
        }

        // 2. API Đăng nhập
        [HttpPost("dang-nhap")]
        public async Task<IActionResult> DangNhap(string email, string matKhauGoc)
        {
            // Tìm user theo email
            var user = await _context.KhachHangs.SingleOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return NotFound("Tài khoản không tồn tại!");

            if (user.TrangThai == "locked")
                return Unauthorized("Tài khoản của bạn đã bị khóa!");

            // KIỂM TRA MẬT KHẨU
            bool checkPass = BCrypt.Net.BCrypt.Verify(matKhauGoc, user.MatKhau);

            if (!checkPass)
                return BadRequest("Sai mật khẩu!");

            // TẠO JWT TOKEN KHI ĐĂNG NHẬP THÀNH CÔNG
        

          
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.MaKh.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            
            if (user.VaiTro == "Admin")
            {
                // Cấp thẻ bài Admin
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                // Cấp thẻ bài Khách hàng
                claims.Add(new Claim(ClaimTypes.Role, "KhachHang"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("LXR_Hotel_Super_Secret_Key_At_Least_32_Chars_Long_2026!!!");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), 
                Expires = DateTime.UtcNow.AddHours(2), 
                Issuer = "LuxuryHotelAPI",
                Audience = "LuxuryHotelClients",
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
        // Class phụ để nhận dữ liệu cập nhật
        public class CapNhatProfileRequest
        {
            public string HoTen { get; set; } = null!;
            public string SoDienThoai { get; set; } = null!;
            public string Cmnd { get; set; } = null!;
        }

      
        // API 3: LẤY THÔNG TIN HỒ SƠ
      
        [HttpGet("thong-tin")]
        [Authorize]
        public async Task<IActionResult> GetThongTinCaNhan()
        {
            // Đọc ID người dùng từ Token (ClaimTypes.NameIdentifier)
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized("Token không hợp lệ.");

            // Tìm đúng  khách hàng đó trong DB
            var user = await _context.KhachHangs
                .Where(k => k.MaKh == userId)
                .Select(k => new { k.Email, k.HoTen, k.SoDienThoai, k.Cmnd })
                .FirstOrDefaultAsync();

            if (user == null) return NotFound("Không tìm thấy dữ liệu người dùng.");

            return Ok(user);
        }

      
        // API 4: CẬP NHẬT HỒ SƠ
      
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
       
        // UC04: QUÊN MẬT KHẨU
     
        [HttpPost("quen-mat-khau")]
        public async Task<IActionResult> QuenMatKhau([FromBody] string email)
        {
            var user = await _context.KhachHangs.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null) return NotFound("Email không tồn tại trong hệ thống!");

         
            string newPassword = "LXR" + new Random().Next(100000, 999999).ToString();

       
            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();

          
            return Ok(new { message = "Đã cấp mật khẩu mới.", matKhauMoi = newPassword });
        }
    }
}