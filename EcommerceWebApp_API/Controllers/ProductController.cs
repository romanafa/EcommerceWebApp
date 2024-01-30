using EcommerceWebApp_API.Data;
using EcommerceWebApp_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceWebApp_API.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            // Fetch all products from the database and return them
            _response.Response = _db.Products;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            // Check if product id is valid
            if(productId == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Invalid product id");
                return BadRequest(_response);
            }

            // If id is valid, fetch and return the product from database
            Product product = _db.Products.FirstOrDefault(p => p.ProductId == productId);
            if(product == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Errors.Add("Product not found");
                return NotFound(_response);
            }

            _response.Response = product;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
