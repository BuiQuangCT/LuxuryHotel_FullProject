using System;
using System.Collections.Generic;

namespace lxrhotel.API.Models;

public partial class HinhAnh
{
    public int MaAnh { get; set; }

    public string? MaPhong { get; set; }

    public string MaKs { get; set; } = null!;

    public string DuongDan { get; set; } = null!;

    public string? NguonGoc { get; set; }

    public DateTime? NgayThem { get; set; }

    public virtual KhachSan MaKsNavigation { get; set; } = null!;

    public virtual Phong? MaPhongNavigation { get; set; }
}
