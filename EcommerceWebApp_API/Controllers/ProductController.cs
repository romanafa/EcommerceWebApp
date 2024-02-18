using AutoMapper;
using EcommerceWebApp_API.Data;
using EcommerceWebApp_API.Models.Product;
using EcommerceWebApp_API.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EcommerceWebApp_API.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public ProductController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadOnlyDto>>> GetProducts()
        {
            if (_db.Products == null)
            {
                return NotFound("No products found");
            }

            try
            {
                // Fetch all products from the database and return them
                var products = await _db.Products
                    .Include(pc => pc.ProductCategories)
                    .ThenInclude(c => c.Category)
                    .ToListAsync();
                _response.StatusCode = HttpStatusCode.OK;

                var productsDto = _mapper.Map<IEnumerable<ProductReadOnlyDto>>(products);

                _response.Response = productsDto;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }          
        }

        [HttpGet("{productId:int}")]
        public async Task<ActionResult<ProductReadOnlyDto>> GetProduct(int productId)
        {
            // Check if product id is valid
            if(productId == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Invalid product id");
                return BadRequest(_response);
            }

            try
            {
                // Fetch product from the database
                var product = await _db.Products
                    .Include(pc => pc.ProductCategories)
                    .ThenInclude(c => c.Category)
                    .Where(p => p.ProductId == productId)
                    .FirstOrDefaultAsync();

                // Check if product exists
                if (product == null)
                {
                    // Return not found response
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Errors.Add("Product not found");
                    return NotFound(_response);
                }

                // Return product and OK response
                _response.Response = product;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }                     
        }

    }
}
