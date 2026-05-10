using System;
using System.Collections.Generic;

namespace lxrhotel.API.Models;

public partial class DatCoc
{
    public int MaDatCoc { get; set; }

    public int MaDatPhong { get; set; }

    public decimal SoTienCoc { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayDatCoc { get; set; }

    public virtual DatPhong MaDatPhongNavigation { get; set; } = null!;
}
