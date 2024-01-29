using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApp_API.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductTitle { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        [Required]
        public int Stock { get; set; }
        public string Colour { get; set; }
        public string Size { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<Category> Categories { get; } = [];

    }
}
