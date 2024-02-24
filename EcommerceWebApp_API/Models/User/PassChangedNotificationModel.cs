using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApp_API.Models.User
{
    public class PassChangedNotificationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Add other properties to notify
       
    }
}
