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
        public string Password { get; set; }

       

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }

}
