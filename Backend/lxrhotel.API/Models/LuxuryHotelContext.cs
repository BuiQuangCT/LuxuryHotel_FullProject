using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace lxrhotel.API.Models;

public partial class LuxuryHotelContext : DbContext
{
    public LuxuryHotelContext()
    {
    }

    public LuxuryHotelContext(DbContextOptions<LuxuryHotelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DanhGium> DanhGia { get; set; }

    public virtual DbSet<DatCoc> DatCocs { get; set; }

    public virtual DbSet<DatPhong> DatPhongs { get; set; }

    public virtual DbSet<GiaoDich> GiaoDiches { get; set; }

    public virtual DbSet<HinhAnh> HinhAnhs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhachSan> KhachSans { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-3S88H9S1;Database=LuxuryHotel;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DanhGium>(entity =>
        {
            entity.HasKey(e => e.MaDg).HasName("PK__DANH_GIA__7A3EF40E22E5C8EE");

            entity.ToTable("DANH_GIA");

            entity.Property(e => e.MaDg).HasColumnName("maDG");
            entity.Property(e => e.DiemSo).HasColumnName("diemSo");
            entity.Property(e => e.MaKh).HasColumnName("maKH");
            entity.Property(e => e.MaKs)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maKS");
            entity.Property(e => e.NoiDung)
                .HasColumnType("ntext")
                .HasColumnName("noiDung");
            entity.Property(e => e.ThoiGian)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("thoiGian");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Hiển thị")
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DANH_GIA__maKH__7F2BE32F");

            entity.HasOne(d => d.MaKsNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaKs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DANH_GIA__maKS__00200768");
        });

        modelBuilder.Entity<DatCoc>(entity =>
        {
            entity.HasKey(e => e.MaDatCoc).HasName("PK__DAT_COC__C539518AF53AB246");

            entity.ToTable("DAT_COC");

            entity.HasIndex(e => e.MaDatPhong, "UQ__DAT_COC__67884E4EEA859A1C").IsUnique();

            entity.Property(e => e.MaDatCoc).HasColumnName("maDatCoc");
            entity.Property(e => e.MaDatPhong).HasColumnName("maDatPhong");
            entity.Property(e => e.NgayDatCoc)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayDatCoc");
            entity.Property(e => e.SoTienCoc)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("soTienCoc");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Chờ TT")
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaDatPhongNavigation).WithOne(p => p.DatCoc)
                .HasForeignKey<DatCoc>(d => d.MaDatPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DAT_COC__maDatPh__6E01572D");
        });

        modelBuilder.Entity<DatPhong>(entity =>
        {
            entity.HasKey(e => e.MaDatPhong).HasName("PK__DAT_PHON__67884E4FD9524089");

            entity.ToTable("DAT_PHONG", tb => tb.HasTrigger("trg_CheckBookingConflict"));

            entity.HasIndex(e => e.MaXacNhan, "UQ__DAT_PHON__EDFABF1121F079EA").IsUnique();

            entity.Property(e => e.MaDatPhong).HasColumnName("maDatPhong");
            entity.Property(e => e.MaKh).HasColumnName("maKH");
            entity.Property(e => e.MaPhong)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maPhong");
            entity.Property(e => e.MaXacNhan)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maXacNhan");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayDat");
            entity.Property(e => e.NgayNhan)
                .HasColumnType("datetime")
                .HasColumnName("ngayNhan");
            entity.Property(e => e.NgayTra)
                .HasColumnType("datetime")
                .HasColumnName("ngayTra");
            entity.Property(e => e.SoNguoi).HasColumnName("soNguoi");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Chờ xác nhận")
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DatPhongs)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DAT_PHONG__maKH__656C112C");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.DatPhongs)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DAT_PHONG__maPho__66603565");
        });

        modelBuilder.Entity<GiaoDich>(entity =>
        {
            entity.HasKey(e => e.MaGd).HasName("PK__GIAO_DIC__7A3E2D67882A131E");

            entity.ToTable("GIAO_DICH");

            entity.Property(e => e.MaGd).HasColumnName("maGD");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(300)
                .HasColumnName("ghiChu");
            entity.Property(e => e.MaGdcong)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("maGDCong");
            entity.Property(e => e.MaHd).HasColumnName("maHD");
            entity.Property(e => e.PhuongThuc)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("phuongThuc");
            entity.Property(e => e.SoTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("soTien");
            entity.Property(e => e.ThoiGian)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("thoiGian");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.GiaoDiches)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GIAO_DICH__maHD__7A672E12");
        });

        modelBuilder.Entity<HinhAnh>(entity =>
        {
            entity.HasKey(e => e.MaAnh).HasName("PK__HINH_ANH__184D773600D56621");

            entity.ToTable("HINH_ANH");

            entity.Property(e => e.MaAnh).HasColumnName("maAnh");
            entity.Property(e => e.DuongDan)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("duongDan");
            entity.Property(e => e.MaKs)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maKS");
            entity.Property(e => e.MaPhong)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maPhong");
            entity.Property(e => e.NgayThem)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayThem");
            entity.Property(e => e.NguonGoc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("KhachSan")
                .HasColumnName("nguonGoc");

            entity.HasOne(d => d.MaKsNavigation).WithMany(p => p.HinhAnhs)
                .HasForeignKey(d => d.MaKs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HINH_ANH__maKS__06CD04F7");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.HinhAnhs)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK__HINH_ANH__maPhon__05D8E0BE");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HOA_DON__7A3E14861C7B931E");

            entity.ToTable("HOA_DON");

            entity.HasIndex(e => e.MaDatPhong, "UQ__HOA_DON__67884E4E2E33D464").IsUnique();

            entity.Property(e => e.MaHd).HasColumnName("maHD");
            entity.Property(e => e.MaDatPhong).HasColumnName("maDatPhong");
            entity.Property(e => e.NgayXuatHd)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayXuatHD");
            entity.Property(e => e.SoTienConLai)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("soTienConLai");
            entity.Property(e => e.SoTienDaCoc)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("soTienDaCoc");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");
            entity.Property(e => e.TrangThaiTt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Chưa TT")
                .HasColumnName("trangThaiTT");

            entity.HasOne(d => d.MaDatPhongNavigation).WithOne(p => p.HoaDon)
                .HasForeignKey<HoaDon>(d => d.MaDatPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HOA_DON__maDatPh__74AE54BC");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KHACH_HA__7A3ECFE48CB5CC92");

            entity.ToTable("KHACH_HANG");

            entity.HasIndex(e => e.Email, "UQ__KHACH_HA__AB6E61643B1422CA").IsUnique();

            entity.Property(e => e.MaKh).HasColumnName("maKH");
            entity.Property(e => e.Cmnd)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("cmnd");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.HoTen)
                .HasMaxLength(100)
                .HasColumnName("hoTen");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("matKhau");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("active")
                .HasColumnName("trangThai");
        });

        modelBuilder.Entity<KhachSan>(entity =>
        {
            entity.HasKey(e => e.MaKs).HasName("PK__KHACH_SA__7A3ECFF978E71288");

            entity.ToTable("KHACH_SAN");

            entity.Property(e => e.MaKs)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maKS");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(300)
                .HasColumnName("diaChi");
            entity.Property(e => e.DiaDiem)
                .HasMaxLength(200)
                .HasColumnName("diaDiem");
            entity.Property(e => e.MoTa)
                .HasColumnType("ntext")
                .HasColumnName("moTa");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");
            entity.Property(e => e.SaoHang).HasColumnName("saoHang");
            entity.Property(e => e.TenKs)
                .HasMaxLength(100)
                .HasColumnName("tenKS");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.MaPhong).HasName("PK__PHONG__4CD55E10A89A8655");

            entity.ToTable("PHONG");

            entity.Property(e => e.MaPhong)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maPhong");
            entity.Property(e => e.DienTich).HasColumnName("dienTich");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("gia");
            entity.Property(e => e.LoaiPhong)
                .HasMaxLength(50)
                .HasColumnName("loaiPhong");
            entity.Property(e => e.MaKs)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maKS");
            entity.Property(e => e.SucChua).HasColumnName("sucChua");
            entity.Property(e => e.TienNghi)
                .HasMaxLength(500)
                .HasColumnName("tienNghi");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Trống")
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaKsNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.MaKs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PHONG__maKS__5DCAEF64");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
