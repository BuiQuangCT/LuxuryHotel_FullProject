using System;
using System.Collections.Generic;

namespace lxrhotel.API.Models;

public partial class Phong
{
    public string MaPhong { get; set; } = null!;

    public string MaKs { get; set; } = null!;

    public string LoaiPhong { get; set; } = null!;

    public decimal Gia { get; set; }

    public int DienTich { get; set; }

    public string? TienNghi { get; set; }

    public int SucChua { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();

    public virtual ICollection<HinhAnh> HinhAnhs { get; set; } = new List<HinhAnh>();

    public virtual KhachSan MaKsNavigation { get; set; } = null!;
}
