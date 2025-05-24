using System.ComponentModel.DataAnnotations;

namespace QLNS.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên chủ đề")]
        [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự")]
        public string CategoryName { get; set; }

        public List<Book> Books { get; set; }
    }
}
