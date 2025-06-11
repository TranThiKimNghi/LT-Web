// Webbanhang.Models.ChiTietDangKy.cs
// KHÔNG dùng using System.ComponentModel.DataAnnotations; ở đây vì không dùng [Key]
using System.ComponentModel.DataAnnotations.Schema;

namespace Sinhvien.Models
{
    public class ChiTietDangKy
    {
        // Các thuộc tính này sẽ tạo thành khóa chính kép, được định nghĩa trong DbContext
        [ForeignKey("DangKy")]
        public int MaDK { get; set; }

        [ForeignKey("HocPhan")]
        [Column(TypeName = "char(6)")] // Đảm bảo kiểu dữ liệu khớp với DB
        public string MaHP { get; set; }

        // Navigation properties
        public DangKy? DangKy { get; set; }
        public HocPhan? HocPhan { get; set; }
    }
}