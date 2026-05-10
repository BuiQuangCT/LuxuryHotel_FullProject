using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using lxrhotel.API.Models; 

namespace lxrhotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] //  Admin mới lọt qua được
    public class AdminController : ControllerBase
    {
        private readonly LuxuryHotelContext _context;

        public AdminController(LuxuryHotelContext context)
        {
            _context = context;
        }

        
        // UC-ADMIN-01: CẬP NHẬT TRẠNG THÁI PHÒNG
       
        [HttpPut("cap-nhat-trang-thai/{maPhong}")]
        public async Task<IActionResult> CapNhatTrangThaiPhong(string maPhong, [FromBody] string status)
        {
            var phong = await _context.Phongs.FindAsync(maPhong);
            if (phong == null)
            {
                return NotFound(new { message = "Không tìm thấy phòng này." });
            }

          
            phong.TrangThai = status;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã cập nhật trạng thái phòng {maPhong} thành {status}." });
        }

        
        // UC-ADMIN-02: THỐNG KÊ DOANH THU
        
        [HttpGet("thong-ke-doanh-thu")]
        public async Task<IActionResult> ThongKeDoanhThu(int thang, int nam)
        {
          
            var doanhThu = await _context.GiaoDiches
                .Where(gd => gd.ThoiGian.Value.Month == thang && gd.ThoiGian.Value.Year == nam && gd.TrangThai == "Success")
                .SumAsync(gd => gd.SoTien);

           
            var tongDonHang = await _context.DatPhongs
                .CountAsync(dp => dp.NgayDat != null && dp.NgayDat.Value.Month == thang && dp.NgayDat.Value.Year == nam);

            return Ok(new
            {
                thoiGian = $"{thang}/{nam}",
                tongDoanhThu = doanhThu,
                soLuongDonHang = tongDonHang,
                donVi = "VND"
            });
        }

     
        // UC-ADMIN-03: XEM DANH SÁCH ĐẶT PHÒNG
      
        [HttpGet("danh-sach-dat-phong")]
        public async Task<IActionResult> GetTatCaDatPhong()
        {
            // Lấy danh sách đặt phòng, sắp xếp đơn mới nhất lên đầu trang
            var list = await _context.DatPhongs
                .OrderByDescending(x => x.NgayDat)
                .ToListAsync();

            return Ok(list);
        }
       
        // UC-ADMIN-04: CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG 
        
        [HttpPut("cap-nhat-don/{maDatPhong}")]
        public async Task<IActionResult> CapNhatTrangThaiDon(int maDatPhong, [FromBody] string status)
        {
            var don = await _context.DatPhongs.FindAsync(maDatPhong);
            if (don == null) return NotFound(new { message = "Không tìm thấy đơn đặt phòng." });

            don.TrangThai = status;

           
            if (status == "Success")
            {
                var phong = await _context.Phongs.FindAsync(don.MaPhong);
                if (phong != null) phong.TrangThai = "Đã đặt";
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Đã cập nhật đơn #{maDatPhong} thành {status}" });
        }
       
        // LẤY DANH SÁCH TOÀN BỘ PHÒNG
       
        [HttpGet("danh-sach-phong")]
        public async Task<IActionResult> GetDanhSachPhong()
        {
            var list = await _context.Phongs.Select(p => new {
                p.MaPhong,
                TenKhachSan = p.MaKsNavigation.TenKs,
                p.LoaiPhong,
                p.Gia,
                p.TrangThai
            }).ToListAsync();
            return Ok(list);
        }

        // LẤY DANH SÁCH KHÁCH HÀNG 
      
        [HttpGet("danh-sach-khach-hang")]
        public async Task<IActionResult> GetDanhSachKhachHang()
        {
            var list = await _context.KhachHangs
                .Where(k => k.VaiTro != "Admin")
                .Select(k => new { k.MaKh, k.HoTen, k.Email, k.SoDienThoai, k.TrangThai })
                .ToListAsync();
            return Ok(list);
        }

       
        // KHÓA / MỞ KHÓA TÀI KHOẢN KHÁCH HÀNG
       
        [HttpPut("khoa-tai-khoan/{maKh}")]
        public async Task<IActionResult> KhoaTaiKhoan(int maKh)
        {
            var kh = await _context.KhachHangs.FindAsync(maKh);
            if (kh == null) return NotFound("Không tìm thấy khách hàng.");

            // Đảo ngược trạng thái
            kh.TrangThai = (kh.TrangThai == "active") ? "locked" : "active";
            await _context.SaveChangesAsync();
            
            return Ok(new { message = $"Đã {((kh.TrangThai == "locked") ? "khóa" : "mở khóa")} tài khoản thành công!" });
        }
    }
}