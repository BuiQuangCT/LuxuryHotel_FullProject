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

            // LOGIC MỚI: Kiểm tra xem phòng có đang được sử dụng không
            var today = DateTime.Now.Date;
            var isOccupied = await _context.DatPhongs.AnyAsync(dp => 
                dp.MaPhong == maPhong &&
                dp.TrangThai == "Success" &&
                dp.NgayNhan.Date <= today &&
                dp.NgayTra.Date > today
            );

            if (isOccupied)
            {
                // Nếu phòng đang có khách, không cho phép thay đổi và trả về lỗi
                return BadRequest("Không thể thay đổi trạng thái. Phòng này hiện đang có khách ở.");
            }
            
            phong.TrangThai = trangThaiMoi;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã cập nhật trạng thái phòng {maPhong} thành {trangThaiMoi}." });
        }

        
        // UC-ADMIN-02: THỐNG KÊ DOANH THU
        
        [HttpGet("thong-ke-doanh-thu")]
        public async Task<IActionResult> ThongKeDoanhThu(int thang, int nam)
        {
       // Cải tiến: Lấy tất cả đơn hàng thành công trong kỳ để tính toán, đảm bảo sự đồng bộ.
            var successfulBookingsInPeriod = await (from dp in _context.DatPhongs
                                                    join dc in _context.DatCocs on dp.MaDatPhong equals dc.MaDatPhong
                                                    where dc.NgayDatCoc.Value.Month == thang &&
                                                          dc.NgayDatCoc.Value.Year == nam &&
                                                          dp.TrangThai == "Success"
                                                    select dp).ToListAsync();

            // Tính toán từ danh sách đã lọc
            var doanhThu = successfulBookingsInPeriod.Sum(dp => dp.TongTien);
            var soLuongDonHangThanhCong = successfulBookingsInPeriod.Count();

            return Ok(new
            {
                thoiGian = $"{thang}/{nam}",
                tongDoanhThu = doanhThu, // Tổng doanh thu từ các đơn thành công trong kỳ
                soLuongDonHang = soLuongDonHangThanhCong, // Số lượng đơn hàng thành công trong kỳ
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
            // Lấy tất cả các phòng và thông tin khách sạn liên quan
            var allRooms = await _context.Phongs
                .Include(p => p.MaKsNavigation)
                .ToListAsync();

            var today = DateTime.Now.Date;

            // Lấy mã của tất cả các phòng đang được sử dụng HÔM NAY
            var occupiedRoomIds = await _context.DatPhongs
                .Where(dp => dp.TrangThai == "Success" && dp.NgayNhan.Date <= today && dp.NgayTra.Date > today)
                .Select(dp => dp.MaPhong)
                .Distinct() // Đảm bảo không có mã phòng trùng lặp
                .ToListAsync();

            // Xử lý logic để xác định trạng thái cuối cùng
            var result = allRooms.Select(p => {
                string finalStatus;
                if (occupiedRoomIds.Contains(p.MaPhong))
                {
                    // Nếu phòng có trong danh sách đang sử dụng, trạng thái là "Đang sử dụng"
                    finalStatus = "Đang sử dụng";
                }
                else
                {
                    // Ngược lại, lấy trạng thái từ DB (Trống, Bảo trì, Đang dọn dẹp)
                    finalStatus = p.TrangThai;
                }

                return new {
                    p.MaPhong,
                    TenKhachSan = p.MaKsNavigation.TenKs,
                    p.LoaiPhong,
                    p.Gia,
                    p.SucChua,
                    TrangThai = finalStatus
                };
            }).ToList();

            return Ok(result);
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