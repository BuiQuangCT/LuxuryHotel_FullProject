using System;
using System.Collections.Generic;

namespace lxrhotel.API.Models;

public partial class DatPhong
{
    public int MaDatPhong { get; set; }

    public int MaKh { get; set; }

    public string MaPhong { get; set; } = null!;

    public DateTime NgayNhan { get; set; }

    public DateTime NgayTra { get; set; }

    public int SoNguoi { get; set; }

    public decimal TongTien { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayDat { get; set; }

    public string? MaXacNhan { get; set; }

    public virtual DatCoc? DatCoc { get; set; }

    public virtual HoaDon? HoaDon { get; set; }

    public virtual KhachHang MaKhNavigation { get; set; } = null!;

    public virtual Phong MaPhongNavigation { get; set; } = null!;
}
