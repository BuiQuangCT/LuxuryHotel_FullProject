using System;
using System.Collections.Generic;

namespace lxrhotel.API.Models;

public partial class DanhGium
{
    public int MaDg { get; set; }

    public int MaKh { get; set; }

    public string MaKs { get; set; } = null!;

    public int DiemSo { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? ThoiGian { get; set; }

    public string? TrangThai { get; set; }

    public virtual KhachHang MaKhNavigation { get; set; } = null!;

    public virtual KhachSan MaKsNavigation { get; set; } = null!;
}
