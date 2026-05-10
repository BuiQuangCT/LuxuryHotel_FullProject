using System;
using System.Collections.Generic;

namespace lxrhotel.API.Models;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public int MaDatPhong { get; set; }

    public decimal TongTien { get; set; }

    public decimal? SoTienDaCoc { get; set; }

    public decimal SoTienConLai { get; set; }

    public DateTime? NgayXuatHd { get; set; }

    public string? TrangThaiTt { get; set; }

    public virtual ICollection<GiaoDich> GiaoDiches { get; set; } = new List<GiaoDich>();

    public virtual DatPhong MaDatPhongNavigation { get; set; } = null!;
}
