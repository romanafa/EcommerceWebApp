using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApp_API.Models.User
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
