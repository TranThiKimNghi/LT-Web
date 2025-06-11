// Trong ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using Sinhvien.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Đảm bảo bạn có các DbSet cho tất cả các Model của mình
    public DbSet<ChiTietDangKy> ChiTietDangKys { get; set; }
    public DbSet<DangKy> DangKys { get; set; }
    public DbSet<HocPhan> HocPhans { get; set; }
    public DbSet<NganhHoc> NganhHocs { get; set; }
    public DbSet<SinhVien> SinhViens { get; set; }
    // ... các DbSet khác nếu có

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Luôn gọi base.OnModelCreating ĐẦU TIÊN nếu bạn dùng IdentityDbContext
        base.OnModelCreating(modelBuilder);

        // *** Cấu hình khóa chính kép cho ChiTietDangKy ***
        modelBuilder.Entity<ChiTietDangKy>()
            .HasKey(ctdk => new { ctdk.MaDK, ctdk.MaHP }); // Đây là dòng quan trọng nhất

        // *** Cấu hình các mối quan hệ cho ChiTietDangKy ***
        modelBuilder.Entity<ChiTietDangKy>()
            .HasOne(ctdk => ctdk.DangKy)
            .WithMany(dk => dk.ChiTietDangKys) // Giả định DangKy có ICollection<ChiTietDangKy> ChiTietDangKys { get; set; }
            .HasForeignKey(ctdk => ctdk.MaDK);

        modelBuilder.Entity<ChiTietDangKy>()
            .HasOne(ctdk => ctdk.HocPhan)
            .WithMany(hp => hp.ChiTietDangKys) // Giả định HocPhan có ICollection<ChiTietDangKy> ChiTietDangKys { get; set; }
            .HasForeignKey(ctdk => ctdk.MaHP);

        // --- Cấu hình các Model khác của bạn dựa trên schema SQL ---

        // Cấu hình NganhHoc
        modelBuilder.Entity<NganhHoc>()
            .Property(nh => nh.MaNganh)
            .HasColumnType("char(4)");
        // Cấu hình SinhVien
        modelBuilder.Entity<SinhVien>()
            .Property(sv => sv.MaSV)
            .HasColumnType("char(10)");
        modelBuilder.Entity<SinhVien>()
            .Property(sv => sv.MaNganh)
            .HasColumnType("char(4)"); // Foreign Key to NganhHoc
        modelBuilder.Entity<SinhVien>()
            .HasOne(sv => sv.NganhHoc)
            .WithMany() // Nếu NganhHoc không có ICollection<SinhVien>, dùng WithMany()
            .HasForeignKey(sv => sv.MaNganh);
        // Cấu hình HocPhan
        modelBuilder.Entity<HocPhan>()
            .Property(hp => hp.MaHP)
            .HasColumnType("char(6)");
        // Cấu hình DangKy
        modelBuilder.Entity<DangKy>()
            .Property(dk => dk.MaDK)
            .ValueGeneratedOnAdd(); // Giả định MaDK là Identity(1,1) trong DB
        modelBuilder.Entity<DangKy>()
            .Property(dk => dk.MaSV)
            .HasColumnType("char(10)"); // Foreign Key to SinhVien
        modelBuilder.Entity<DangKy>()
            .HasOne(dk => dk.SinhVien)
            .WithMany() // Nếu SinhVien không có ICollection<DangKy>, dùng WithMany()
            .HasForeignKey(dk => dk.MaSV);
    }
}