using Microsoft.AspNetCore.Identity;

namespace EcommerceWebApp_API.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }

        public string phone { get; set; }


        // Examples of information that might be needed! More to come :) 
        public DateTime AccountCreated { get; set; }

        public string ProfilePictureUrl { get; set; }



    }
}
