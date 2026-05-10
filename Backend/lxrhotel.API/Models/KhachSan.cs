using System;
using System.Collections.Generic;

namespace lxrhotel.API.Models;

public partial class KhachSan
{
    public string MaKs { get; set; } = null!;

    public string TenKs { get; set; } = null!;

    public string DiaDiem { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? MoTa { get; set; }

    public int? SaoHang { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<HinhAnh> HinhAnhs { get; set; } = new List<HinhAnh>();

    public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
}
