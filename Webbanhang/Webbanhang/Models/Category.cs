using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbanhang.Models
{
    public class Category
    {
       
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên danh muc là bắt buộc"), StringLength(50,
            ErrorMessage = "Tên danh muc không được vượt quá 50 ký tự")]
        public string Name { get; set; }
        public List<Product> Products { get; set; } 
       

    }
}
