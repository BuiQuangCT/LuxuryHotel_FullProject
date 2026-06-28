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
                return BadRequest(new { message = "Phòng đang có người, yêu cầu Check-out trước" });
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
                .Include(dp => dp.MaKhNavigation) // Tải thông tin khách hàng liên quan
                .OrderByDescending(x => x.NgayDat)
                .Select(dp => new {
                    dp.MaDatPhong,
                    dp.MaPhong,
                    TenKhachHang = dp.MaKhNavigation.HoTen, // Lấy tên khách hàng
                    SoDienThoai = dp.MaKhNavigation.SoDienThoai, // Lấy SĐT để tìm kiếm
                    dp.NgayNhan,
                    dp.NgayTra,
                    dp.TongTien,
                    dp.TrangThai,
                    dp.NgayDat
                })
                .ToListAsync();

            return Ok(list);
        }
       
        // UC-ADMIN-04A: DUYỆT ĐƠN HÀNG
        [HttpPut("duyet-don/{maDatPhong}")]
        public async Task<IActionResult> DuyetDon(int maDatPhong)
        {
            var don = await _context.DatPhongs.FindAsync(maDatPhong);
            if (don == null) return NotFound(new { message = "Không tìm thấy đơn đặt phòng." });

            if (don.TrangThai != "Pending")
            {
                return BadRequest(new { message = "Chỉ có thể duyệt đơn ở trạng thái 'Pending'." });
            }

            var oldStatus = don.TrangThai;
            don.TrangThai = "Success";

            // Ghi nhận giao dịch/đặt cọc khi duyệt tay
            var existingDatCoc = await _context.DatCocs.FirstOrDefaultAsync(dc => dc.MaDatPhong == maDatPhong);
            if (existingDatCoc == null)
            {
                _context.DatCocs.Add(new DatCoc
                {
                    MaDatPhong = don.MaDatPhong,
                    SoTienCoc = don.TongTien,
                    TrangThai = "Đã thanh toán",
                    NgayDatCoc = DateTime.Now
                });
            }

            // TODO: Gửi email xác nhận cho khách hàng tại đây

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Đã duyệt thành công đơn #{maDatPhong}." });
        }

        // UC-ADMIN-04B: HỦY ĐƠN HÀNG
        [HttpPut("huy-don/{maDatPhong}")]
        public async Task<IActionResult> HuyDon(int maDatPhong)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var don = await _context.DatPhongs.FindAsync(maDatPhong);
                if (don == null) return NotFound(new { message = "Không tìm thấy đơn đặt phòng." });

                if (don.TrangThai == "Success" || don.TrangThai == "Đã hủy")
                {
                     return BadRequest(new { message = $"Không thể hủy đơn ở trạng thái '{don.TrangThai}'." });
                }

                don.TrangThai = "Đã hủy";

                // Cập nhật lại trạng thái phòng về 'Trống'
                // Logic an toàn: chỉ cập nhật nếu không có đơn nào khác đang chiếm giữ phòng
                var phong = await _context.Phongs.FindAsync(don.MaPhong);
                if (phong != null)
                {
                    var today = DateTime.Now.Date;
                    var isOccupiedByAnotherBooking = await _context.DatPhongs.AnyAsync(dp => 
                        dp.MaPhong == phong.MaPhong &&
                        dp.MaDatPhong != maDatPhong && // Loại trừ đơn đang hủy
                        dp.TrangThai == "Success" &&
                        dp.NgayNhan.Date <= today &&
                        dp.NgayTra.Date > today
                    );

                    if (!isOccupiedByAnotherBooking) {
                        phong.TrangThai = "Trống";
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = $"Đã hủy thành công đơn #{maDatPhong}." });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Đã có lỗi xảy ra trong quá trình hủy đơn." });
            }
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