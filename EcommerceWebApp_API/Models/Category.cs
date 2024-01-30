using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApp_API.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryTitle { get; set; }
        public bool IsActive { get; set; }

        public List<Product> Products { get; } = [];
    }
}
