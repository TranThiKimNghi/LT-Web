using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sinhvien.Models
{
    public class DangKy
    {
        [Key] // Denotes the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Corresponds to identity(1,1)
        [DisplayName("Mã đăng ký")] // Display name for UI
        public int MaDK { get; set; }

        [DisplayName("Ngày đăng ký")] // Display name for UI
        public DateTime NgayDK { get; set; }

        [ForeignKey("SinhVien")] // Corresponds to references SinhVien(MaSV)
        [Column(TypeName = "char(10)")] // Specifies the database column type
        [DisplayName("Mã sinh viên")] // Display name for UI
        public string MaSV { get; set; }

        // Navigation property for the foreign key relationship
        public SinhVien? SinhVien { get; set; } // Assuming SinhVien is another model

        // Optional: Navigation property for the many-to-many relationship with HocPhan through ChiTietDangKy
        public ICollection<ChiTietDangKy>? ChiTietDangKys { get; set; }
    }
}