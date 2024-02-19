using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace EcommerceWebApp_API.Models.User
{
    public class UserRegistrationModel : IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            var regex = new Regex(passwordPattern);

            if (!regex.IsMatch(Password))
            {
                yield return new ValidationResult(
                    "Password must be at least 8 characters long, contain at least one digit, one uppercase letter, one lowercase letter, and one special character.",
                    new[] { nameof(Password) });
            }
        }
    }

}
