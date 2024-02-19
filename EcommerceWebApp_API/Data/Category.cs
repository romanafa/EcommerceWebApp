using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWebApp_API.Data
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryTitle { get; set; }
        public bool IsActive { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

        [NotMapped]
        public IEnumerable<Product> Products
        {
            get => ProductCategories?.Select(p => p.Product);
        }
    }
}
