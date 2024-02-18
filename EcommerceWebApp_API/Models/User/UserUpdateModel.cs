using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApp_API.Models.User
{

    public class UserUpdateModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        // Add other properties to update
    }

}
