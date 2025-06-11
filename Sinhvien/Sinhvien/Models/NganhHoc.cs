using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sinhvien.Models
{
    public class NganhHoc
    {
        [Key] // Denotes the primary key
        [Column(TypeName = "char(4)")] // Specifies the database column type
        [DisplayName("Mã ngành")] // Display name for UI purposes
        public string MaNganh { get; set; }

        [StringLength(30, ErrorMessage = "Tên ngành không được vượt quá 30 ký tự")] // Corresponds to nvarchar(30)
        [DisplayName("Tên ngành")] // Display name for UI purposes
        public string TenNganh { get; set; }

        // Optional: Navigation property to hold a collection of SinhVien (Students)
        // if you have a relationship defined in your database where NganhHoc has many SinhVien
        public ICollection<SinhVien>? SinhViens { get; set; }
    }
}