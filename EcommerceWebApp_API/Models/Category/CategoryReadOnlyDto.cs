using EcommerceWebApp_API.Models.Product;

namespace EcommerceWebApp_API.Models.Category
{
    public class CategoryReadOnlyDto
    {
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public bool IsActive { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
