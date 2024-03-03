using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EcommerceWebApp_API.Models.Category
{
    public class CategoryUpdateDto
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryTitle { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
