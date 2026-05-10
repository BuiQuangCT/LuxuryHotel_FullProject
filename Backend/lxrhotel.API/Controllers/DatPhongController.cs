using Microsoft.AspNetCore.Mvc;
using lxrhotel.API.Models;
using Microsoft.EntityFrameworkCore;

namespace lxrhotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class DatPhongController : ControllerBase
    {
        private readonly LuxuryHotelContext _context;

        public DatPhongController(LuxuryHotelContext context)
        {
            _context = context;
        }

       
        public class TaoDonRequest
        {
            public int MaKh { get; set; }
            public string MaPhong { get; set; } = null!;
            public DateTime NgayNhan { get; set; }
            public DateTime NgayTra { get; set; }
            public decimal TongTien { get; set; }
        }

        [HttpPost("tao-don")]
        public async Task<IActionResult> TaoDonDatPhong([FromBody] TaoDonRequest request)
        {
           
            var donMoi = new DatPhong
            {
                MaKh = request.MaKh,
                MaPhong = request.MaPhong,
                NgayDat = DateTime.Now,
                NgayNhan = request.NgayNhan,
                NgayTra = request.NgayTra,
                TongTien = request.TongTien,
                TrangThai = "Pending",
                SoNguoi = 1,
                
                MaXacNhan = Guid.NewGuid().ToString().Substring(0, 8)
            };

            _context.DatPhongs.Add(donMoi);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tạo đơn đặt phòng thành công!", maDatPhong = donMoi.MaDatPhong });

        }
       
        // UC09: LẤY LỊCH SỬ ĐẶT PHÒNG CỦA KHÁCH
        
        [HttpGet("lich-su/{maKh}")]
        public async Task<IActionResult> GetLichSu(int maKh)
        {
            var lichSu = await _context.DatPhongs
                .Where(dp => dp.MaKh == maKh)
                .OrderByDescending(dp => dp.NgayDat)
                .Select(dp => new {
                    dp.MaDatPhong,
                    dp.MaPhong,
                    dp.NgayNhan,
                    dp.NgayTra,
                    dp.TongTien,
                    dp.TrangThai
                })
                .ToListAsync();

            return Ok(lichSu);
        }

       
        // UC10: HỦY ĐẶT PHÒNG
        
        [HttpPut("huy-don/{maDatPhong}")]
        public async Task<IActionResult> HuyDon(int maDatPhong)
        {
            var don = await _context.DatPhongs.FindAsync(maDatPhong);
            if (don == null) return NotFound("Không tìm thấy đơn hàng.");

           
            if (don.TrangThai == "Success" || don.TrangThai == "Đã hủy")
                return BadRequest("Không thể hủy đơn hàng ở trạng thái này.");

            don.TrangThai = "Đã hủy";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã hủy đơn đặt phòng thành công." });
        }

    }

}