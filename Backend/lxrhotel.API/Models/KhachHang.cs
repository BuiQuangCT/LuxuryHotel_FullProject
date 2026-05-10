using System;
using System.Collections.Generic;

namespace lxrhotel.API.Models;

public partial class KhachHang
{
    public int MaKh { get; set; } 
    public string? VaiTro { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? Cmnd { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();

   
}
