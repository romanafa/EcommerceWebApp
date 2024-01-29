using System.Net;

namespace EcommerceWebApp_API.Models
{
    // Returns a generic response for all API calls
    public class ApiResponse
    {
        public ApiResponse()
        {
            Errors = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; set; }

        // Defined as object to allow for any type of response
        public object Response { get; set; }
    }
}
