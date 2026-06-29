﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using lxrhotel.API.Models; 

namespace lxrhotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class AdminController : ControllerBase
    {
        private readonly LuxuryHotelContext _context;

        public AdminController(LuxuryHotelContext context)
        {
            _context = context;
        }

        
        
       
        [HttpPut("cap-nhat-trang-thai/{maPhong}")]
        public async Task<IActionResult> CapNhatTrangThaiPhong(string maPhong, [FromQuery] string trangThaiMoi)
        {
            var phong = await _context.Phongs.FindAsync(maPhong);
            if (phong == null)
            {
                return NotFound(new { message = "Không tìm thấy phòng này." });
            }

           
            var today = DateTime.Now.Date;
            var isOccupied = await _context.DatPhongs.AnyAsync(dp => 
                dp.MaPhong == maPhong &&
                dp.TrangThai == "Success" &&
                dp.NgayNhan.Date <= today &&
                dp.NgayTra.Date > today
            );

            if (isOccupied)
            {
               
                return BadRequest(new { message = "Phòng đang có người, yêu cầu Check-out trước" });
            }
            
            phong.TrangThai = trangThaiMoi;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã cập nhật trạng thái phòng {maPhong} thành {trangThaiMoi}." });
        }

        
        
        
        [HttpGet("thong-ke-doanh-thu")]
        public async Task<IActionResult> ThongKeDoanhThu(int thang, int nam)
        {
       
            var successfulBookingsInPeriod = await (from dp in _context.DatPhongs
                                                    join dc in _context.DatCocs on dp.MaDatPhong equals dc.MaDatPhong
                                                    where dc.NgayDatCoc.Value.Month == thang &&
                                                          dc.NgayDatCoc.Value.Year == nam &&
                                                          dp.TrangThai == "Success"
                                                    select dp).ToListAsync();

            
            var doanhThu = successfulBookingsInPeriod.Sum(dp => dp.TongTien);
            var soLuongDonHangThanhCong = successfulBookingsInPeriod.Count();

            return Ok(new
            {
                thoiGian = $"{thang}/{nam}",
                tongDoanhThu = doanhThu,
                soLuongDonHang = soLuongDonHangThanhCong,
                donVi = "VND"
            });
        }

     
       
      
        [HttpGet("danh-sach-dat-phong")]
        public async Task<IActionResult> GetTatCaDatPhong()
        {
            
            var list = await _context.DatPhongs
                .Include(dp => dp.MaKhNavigation)
                .OrderByDescending(x => x.NgayDat)
                .Select(dp => new {
                    dp.MaDatPhong,
                    dp.MaPhong,
                    TenKhachHang = dp.MaKhNavigation.HoTen, 
                    SoDienThoai = dp.MaKhNavigation.SoDienThoai, 
                    dp.NgayNhan,
                    dp.NgayTra,
                    dp.TongTien,
                    dp.TrangThai,
                    dp.NgayDat
                })
                .ToListAsync();

            return Ok(list);
        }
       
        
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

            

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Đã duyệt thành công đơn #{maDatPhong}." });
        }

        
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

                
                var phong = await _context.Phongs.FindAsync(don.MaPhong);
                if (phong != null)
                {
                    var today = DateTime.Now.Date;
                    var isOccupiedByAnotherBooking = await _context.DatPhongs.AnyAsync(dp => 
                        dp.MaPhong == phong.MaPhong &&
                        dp.MaDatPhong != maDatPhong && 
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
       
        
       
        [HttpGet("danh-sach-phong")]
        public async Task<IActionResult> GetDanhSachPhong()
        {
            
            var allRooms = await _context.Phongs
                .Include(p => p.MaKsNavigation)
                .ToListAsync();

            var today = DateTime.Now.Date;

            
            var occupiedRoomIds = await _context.DatPhongs
                .Where(dp => dp.TrangThai == "Success" && dp.NgayNhan.Date <= today && dp.NgayTra.Date > today)
                .Select(dp => dp.MaPhong)
                .Distinct() 
                .ToListAsync();

           
            var result = allRooms.Select(p => {
                string finalStatus;
                if (occupiedRoomIds.Contains(p.MaPhong))
                {
                   
                    finalStatus = "Đang sử dụng";
                }
                else
                {
                   
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

       
      
        [HttpGet("danh-sach-khach-hang")]
        public async Task<IActionResult> GetDanhSachKhachHang()
        {
            var list = await _context.KhachHangs
                .Where(k => k.VaiTro != "Admin")
                .Select(k => new { k.MaKh, k.HoTen, k.Email, k.SoDienThoai, k.TrangThai })
                .ToListAsync();
            return Ok(list);
        }

       
        
       
        [HttpPut("khoa-tai-khoan/{maKh}")]
        public async Task<IActionResult> KhoaTaiKhoan(int maKh)
        {
            var kh = await _context.KhachHangs.FindAsync(maKh);
            if (kh == null) return NotFound("Không tìm thấy khách hàng.");

           
            kh.TrangThai = (kh.TrangThai == "active") ? "locked" : "active";
            await _context.SaveChangesAsync();
            
            return Ok(new { message = $"Đã {((kh.TrangThai == "locked") ? "khóa" : "mở khóa")} tài khoản thành công!" });
        }
    }
}