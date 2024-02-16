using EcommerceWebApp_API.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApp_API.Models.Product
{
    public class ProductCreateDto
    {
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
        public DateTime CreatedAt = DateTime.Now;

        public int CategoryId { get; set; }
    }
}
