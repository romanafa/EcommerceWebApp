using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApp_API.Models.Category
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryTitle { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
