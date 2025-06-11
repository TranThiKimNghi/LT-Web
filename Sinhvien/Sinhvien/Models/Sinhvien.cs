using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sinhvien.Models
{
   
    public class SinhVien
    {
        [Key] // Denotes primary key
        [Column(TypeName = "char(10)")] // Corresponds to char(10) in SQL
        [DisplayName("Mã sinh viên")] // Display name for MaSV
        public string MaSV { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")] // Corresponds to Not null
        [StringLength(50, ErrorMessage = "Họ tên không được vượt quá 50 ký tự")] // Corresponds to nvarchar(50)
        [DisplayName("Họ tên")] // Display name for HoTen
        public string HoTen { get; set; }

        [StringLength(5, ErrorMessage = "Giới tính không được vượt quá 5 ký tự")] // Corresponds to nvarchar(5)
        [DisplayName("Giới tính")] // Display name for GioiTinh
        public string? GioiTinh { get; set; } // Nullable as it's not marked Not null in SQL

        [DisplayName("Ngày sinh")] // Display name for NgaySinh
        public DateTime? NgaySinh { get; set; } // Nullable as it's not marked Not null in SQL

        [StringLength(50, ErrorMessage = "Đường dẫn hình ảnh không được vượt quá 50 ký tự")] // Corresponds to varchar(50)
        [DisplayName("Hình ảnh")] // Display name for Hinh
        public string? Hinh { get; set; } // Nullable as it's not marked Not null in SQL

        [ForeignKey("NganhHoc")] // Corresponds to references NganhHoc(MaNganh)
        [Column(TypeName = "char(4)")] // Corresponds to char(4) in SQL
        [DisplayName("Mã ngành")] // Display name for MaNganh
        public string MaNganh { get; set; }

        // Navigation property for the foreign key relationship
        public NganhHoc? NganhHoc { get; set; } // Assuming NganhHoc is another model
    }

    // You would also need a NganhHoc (Major) model to represent the referenced table
  
}