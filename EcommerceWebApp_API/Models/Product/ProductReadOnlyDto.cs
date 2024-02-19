using EcommerceWebApp_API.Models.Category;

namespace EcommerceWebApp_API.Models.Product
{
    public class ProductReadOnlyDto
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Colour { get; set; }
        public string Size { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }
        public List<CategoryDto> Categories { get; set; }

    }
}
