using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lxrhotel.API.Models;

namespace lxrhotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongController : ControllerBase
    {
        private readonly LuxuryHotelContext _context;

        public PhongController()
        {
            _context = new LuxuryHotelContext();
        }

      
        // API 1: LẤY DANH SÁCH ĐỊA ĐIỂM
        
        [HttpGet("dia-diem")]
        public IActionResult GetDanhSachDiaDiem()
        {
            var danhSachDiaDiem = _context.KhachSans
                                          .Select(ks => ks.DiaDiem)
                                          .Distinct()
                                          .ToList();

            return Ok(danhSachDiaDiem);
        }

        // API 2: TÌM KIẾM PHÒNG TRỐNG
        
        [HttpGet("tim-kiem")]
        public IActionResult TimKiemPhong(string diaDiem, DateTime ngayNhan, DateTime ngayTra, int soNguoi)
        {
            if (ngayNhan >= ngayTra) return BadRequest("Ngày nhận phải trước ngày trả.");

            var phongTrong = _context.Phongs
                // CHỈNH SỬA: Thêm điều kiện kiểm tra trạng thái phòng phải là "Trống" hoặc "Sẵn sàng"
                .Where(p => (p.TrangThai == "Trống" || p.TrangThai == "Sẵn sàng") && 
                            p.MaKsNavigation.DiaDiem.Contains(diaDiem) && 
                            p.SucChua >= soNguoi)
                .Where(p => !p.DatPhongs.Any(dp =>
                    dp.TrangThai != "Đã hủy" &&
                    dp.NgayNhan < ngayTra &&
                    dp.NgayTra > ngayNhan
                ))
                .Select(p => new {
                    MaPhong = p.MaPhong,
                    TenKhachSan = p.MaKsNavigation.TenKs,
                    LoaiPhong = p.LoaiPhong,
                    Gia = p.Gia,
                    DienTich = p.DienTich,
                    SucChua = p.SucChua,
                    AnhDaiDien = p.HinhAnhs.Select(a => a.DuongDan).FirstOrDefault() 
                })
                .ToList();

            return Ok(phongTrong);
        }
        [HttpGet("tim-kiem-sp")]
        public async Task<IActionResult> TimKiemPhongSP(string diaDiem, DateTime ngayNhan, DateTime ngayTra, int soNguoi)
        {
            if (ngayNhan >= ngayTra) return BadRequest("Ngày nhận phải trước ngày trả.");

            
            var ketQua = await _context.Phongs
                .FromSqlInterpolated($"EXEC sp_TimKiemPhong @DiaDiem={diaDiem}, @NgayNhan={ngayNhan}, @NgayTra={ngayTra}, @SoNguoi={soNguoi}")
                // CHỈNH SỬA: Thêm điều kiện lọc sau khi thực thi Stored Procedure
                .Where(p => p.TrangThai == "Trống" || p.TrangThai == "Sẵn sàng")
                .Select(p => new {
                    p.MaPhong,
                    TenKhachSan = p.MaKsNavigation.TenKs,
                    p.LoaiPhong,
                    p.Gia,
                    p.DienTich,
                    p.SucChua,
                    AnhDaiDien = p.HinhAnhs.Select(a => a.DuongDan).FirstOrDefault()
                })
                .ToListAsync();

            return Ok(ketQua);
        }
        
        // API 3: LẤY CHI TIẾT 1 PHÒNG
      
        [HttpGet("{maPhong}")]
        public IActionResult GetChiTietPhong(string maPhong)
        {
            var chiTiet = _context.Phongs
                .Include(p => p.MaKsNavigation)
                .Include(p => p.HinhAnhs)
                .FirstOrDefault(p => p.MaPhong == maPhong);

            if (chiTiet == null) return NotFound("Không tìm thấy phòng này.");

            var result = new
            {
                MaPhong = chiTiet.MaPhong,
                LoaiPhong = chiTiet.LoaiPhong,
                Gia = chiTiet.Gia,
                TienNghi = chiTiet.TienNghi,
                KhachSan = new
                {
                    TenKS = chiTiet.MaKsNavigation.TenKs,
                    DiaChi = chiTiet.MaKsNavigation.DiaChi,
                    SaoHang = chiTiet.MaKsNavigation.SaoHang
                },
                DanhSachAnh = chiTiet.HinhAnhs.Select(a => a.DuongDan).ToList()
            };

            return Ok(result);

        }
    }
}