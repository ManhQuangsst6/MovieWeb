using Microsoft.EntityFrameworkCore;

namespace MovieWeb.Models;

public partial class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<DanhGiaChung> DanhGiaChungs { get; set; }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<HinhThuc> HinhThucs { get; set; }

    public virtual DbSet<TPhim> TPhims { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TheLoai> TheLoais { get; set; }

    public virtual DbSet<ThoiLuongDaXem> ThoiLuongDaXems { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.MaComment);

            entity.ToTable("Comment");

            entity.Property(e => e.ThoiGianCmt)
                .HasColumnType("datetime")
                .HasColumnName("ThoiGianCMT");

            entity.HasOne(d => d.MaPhimNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MaPhim)
                .HasConstraintName("FK_Comment_tPhim");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MaTaiKhoan)
                .HasConstraintName("FK_Comment_TaiKhoan");
        });

        modelBuilder.Entity<DanhGiaChung>(entity =>
        {
            entity.HasKey(e => new { e.MaTaiKhoan, e.MaPhim });

            entity.ToTable("DanhGiaChung");

            entity.HasOne(d => d.MaPhimNavigation).WithMany(p => p.DanhGiaChungs)
                .HasForeignKey(d => d.MaPhim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DanhGiaChung_tPhim");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.DanhGiaChungs)
                .HasForeignKey(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DanhGiaChung_TaiKhoan");
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => e.MaTapPhim);

            entity.ToTable("Episode");

            entity.Property(e => e.Duonglink).HasMaxLength(500);

            entity.HasOne(d => d.MaPhimNavigation).WithMany(p => p.Episodes)
                .HasForeignKey(d => d.MaPhim)
                .HasConstraintName("FK_Episode_tPhim");
        });

        modelBuilder.Entity<HinhThuc>(entity =>
        {
            entity.HasKey(e => e.MaHinhThuc);

            entity.ToTable("HinhThuc");

            entity.Property(e => e.TenHinhThuc).HasMaxLength(50);
        });

        modelBuilder.Entity<TPhim>(entity =>
        {
            entity.HasKey(e => e.MaPhim);

            entity.ToTable("tPhim");

            entity.Property(e => e.NgayKhoiChieu).HasColumnType("datetime");
            entity.Property(e => e.QuocGia).HasMaxLength(50);
            entity.Property(e => e.TenPhim).HasMaxLength(100);

            entity.HasOne(d => d.MaHinhThucNavigation).WithMany(p => p.TPhims)
                .HasForeignKey(d => d.MaHinhThuc)
                .HasConstraintName("FK_tPhim_HinhThuc");

            entity.HasOne(d => d.MaTheLoaiNavigation).WithMany(p => p.TPhims)
                .HasForeignKey(d => d.MaTheLoai)
                .HasConstraintName("FK_tPhim_TheLoai");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.TPhims)
                .HasForeignKey(d => d.MaTrangThai)
                .HasConstraintName("FK_tPhim_TrangThai");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan);

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.DateUpdate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.PassWord).HasMaxLength(50);
            entity.Property(e => e.SoDt)
                .HasMaxLength(50)
                .HasColumnName("SoDT");
            entity.Property(e => e.TenTaiKhoan).HasMaxLength(50);
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.MaTheLoai);

            entity.ToTable("TheLoai");

            entity.Property(e => e.TenTheLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<ThoiLuongDaXem>(entity =>
        {
            entity.HasKey(e => new { e.MaTaiKhoan, e.MaTapPhim });

            entity.ToTable("ThoiLuongDaXem");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.ThoiLuongDaXems)
                .HasForeignKey(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThoiLuongDaXem_TaiKhoan");

            entity.HasOne(d => d.MaTapPhimNavigation).WithMany(p => p.ThoiLuongDaXems)
                .HasForeignKey(d => d.MaTapPhim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThoiLuongDaXem_Episode");
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai);

            entity.ToTable("TrangThai");

            entity.Property(e => e.TenTrangThai).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
