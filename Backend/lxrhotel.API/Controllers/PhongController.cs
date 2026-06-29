﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lxrhotel.API.Models;

namespace lxrhotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongController : ControllerBase
    {
        private readonly LuxuryHotelContext _context;

        public PhongController(LuxuryHotelContext context)
        {
            _context = context;
        }

      
       
        
        [HttpGet("dia-diem")]
        public async Task<IActionResult> GetDanhSachDiaDiem()
        {
            var danhSachDiaDiem = await _context.KhachSans
                                          .Select(ks => ks.DiaDiem)
                                          .Distinct()
                                          .ToListAsync();

            return Ok(danhSachDiaDiem);
        }

       
        
        [HttpGet("tim-kiem")]
        public async Task<IActionResult> TimKiemPhong(string diaDiem, DateTime ngayNhan, DateTime ngayTra, int soNguoi)
        {
            if (ngayNhan >= ngayTra) return BadRequest("Ngày nhận phải trước ngày trả.");

            var phongTrong = await _context.Phongs
                .Where(p => p.MaKsNavigation.DiaDiem.Contains(diaDiem) && p.SucChua >= soNguoi)
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
                .ToListAsync();

            return Ok(phongTrong);
        }
        [HttpGet("tim-kiem-sp")]
        public async Task<IActionResult> TimKiemPhongSP(string diaDiem, DateTime ngayNhan, DateTime ngayTra, int soNguoi)
        {
            if (ngayNhan >= ngayTra) return BadRequest("Ngày nhận phải trước ngày trả.");

            
            var ketQua = await _context.Phongs
                .FromSqlInterpolated($"EXEC sp_TimKiemPhong @DiaDiem={diaDiem}, @NgayNhan={ngayNhan}, @NgayTra={ngayTra}, @SoNguoi={soNguoi}")
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
        
        
      
        [HttpGet("{maPhong}")]
        public async Task<IActionResult> GetChiTietPhong(string maPhong)
        {
            var chiTiet = await _context.Phongs
                .Include(p => p.MaKsNavigation)
                .Include(p => p.HinhAnhs)
                .FirstOrDefaultAsync(p => p.MaPhong == maPhong);

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