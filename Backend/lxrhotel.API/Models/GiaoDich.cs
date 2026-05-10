using System;
using System.Collections.Generic;

namespace lxrhotel.API.Models;

public partial class GiaoDich
{
    public int MaGd { get; set; }

    public int MaHd { get; set; }

    public decimal SoTien { get; set; }

    public string PhuongThuc { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

    public DateTime? ThoiGian { get; set; }

    public string? MaGdcong { get; set; }

    public string? GhiChu { get; set; }

    public virtual HoaDon MaHdNavigation { get; set; } = null!;
}
