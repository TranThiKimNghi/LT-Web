using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sinhvien.Models
{
    public class HocPhan
    {
        [Key] // Denotes the primary key
        [Column(TypeName = "char(6)")] // Specifies the database column type
        [DisplayName("Mã học phần")] // Display name for UI
        public string MaHP { get; set; }

        [Required(ErrorMessage = "Tên học phần là bắt buộc")] // Corresponds to NOT NULL
        [StringLength(30, ErrorMessage = "Tên học phần không được vượt quá 30 ký tự")] // Corresponds to nvarchar(30)
        [DisplayName("Tên học phần")] // Display name for UI
        public string TenHP { get; set; }

        [DisplayName("Số tín chỉ")] // Display name for UI
        public int SoTinChi { get; set; }

        // Optional: Navigation property for the many-to-many relationship with DangKy through ChiTietDangKy
        public ICollection<ChiTietDangKy>? ChiTietDangKys { get; set; }
    }
}