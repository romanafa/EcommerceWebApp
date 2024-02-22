using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EcommerceWebApp_API.Data;

namespace EcommerceWebApp_API.Models.Product
{
    public class ProductUpdateDto
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        public string ProductTitle { get; set; }
        [Required(ErrorMessage = "Product description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Product price is required")]
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        [Required(ErrorMessage = "Stock is required")]
        public int Stock { get; set; }
        public string Colour { get; set; }
        public string Size { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
