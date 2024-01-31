using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApp_API.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }




        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }

}
