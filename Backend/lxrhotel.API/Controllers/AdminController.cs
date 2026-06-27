﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CapNhatTrangThaiPhong(string maPhong, [FromQuery] string trangThaiMoi)
        {
            var phong = await _context.Phongs.FindAsync(maPhong);
            if (phong == null)
            {
                return NotFound(new { message = "Không tìm thấy phòng này." });
            }

          
            phong.TrangThai = trangThaiMoi;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã cập nhật trạng thái phòng {maPhong} thành {trangThaiMoi}." });
        }

        
        // UC-ADMIN-02: THỐNG KÊ DOANH THU
        
        [HttpGet("thong-ke-doanh-thu")]
        public async Task<IActionResult> ThongKeDoanhThu(int thang, int nam)
        {
            // Sửa lại logic: Tính doanh thu từ bảng Đặt Cọc, nơi ghi nhận các giao dịch thanh toán thành công
            var doanhThu = await _context.DatCocs
                .Where(dc => dc.NgayDatCoc.Value.Month == thang && dc.NgayDatCoc.Value.Year == nam && dc.TrangThai == "Đã thanh toán")
                .SumAsync(dc => (decimal?)dc.SoTienCoc) ?? 0; // Sử dụng (decimal?) để SumAsync hoạt động và ?? 0 để xử lý trường hợp không có giao dịch nào


            // Logic tính tổng đơn hàng giữ nguyên, vì nó đếm số đơn được *tạo* trong tháng
            var tongDonHang = await _context.DatPhongs
                .CountAsync(dp => dp.NgayDat.Value.Month == thang && dp.NgayDat.Value.Year == nam);

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
        public async Task<IActionResult> CapNhatTrangThaiDon(int maDatPhong, [FromQuery] string status)
        {
            var don = await _context.DatPhongs.FindAsync(maDatPhong);
            if (don == null) return NotFound(new { message = "Không tìm thấy đơn đặt phòng." });
 
            var oldStatus = don.TrangThai;
            don.TrangThai = status;
 
            // Khi admin duyệt đơn thủ công (chuyển sang "Success"), ta cần ghi nhận giao dịch này
            // để thống kê doanh thu được chính xác, tương tự như luồng thanh toán online.
            if (status == "Success" && oldStatus == "Pending")
            {
                // Kiểm tra xem đã có bản ghi đặt cọc chưa để tránh tạo trùng lặp
                var existingDatCoc = await _context.DatCocs.FirstOrDefaultAsync(dc => dc.MaDatPhong == maDatPhong);
                if (existingDatCoc == null)
                {
                    var datCoc = new DatCoc
                    {
                        MaDatPhong = don.MaDatPhong,
                        SoTienCoc = don.TongTien, // Giả định admin duyệt là đã thanh toán đủ
                        TrangThai = "Đã thanh toán",
                        NgayDatCoc = DateTime.Now
                    };
                    _context.DatCocs.Add(datCoc);
                }
            }
 
            // Xóa bỏ logic cũ: Việc thay đổi trạng thái phòng khi duyệt đơn là không chính xác.
            // Trạng thái phòng (Trống, Đang sử dụng) nên được xác định động dựa trên ngày nhận/trả phòng
            // của các đơn đã được xác nhận, chứ không nên gán cứng một trạng thái.
 
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
                p.SucChua,
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