using AutoMapper;
using Azure;
using EcommerceWebApp_API.Data;
using EcommerceWebApp_API.Models.Product;
using EcommerceWebApp_API.Services;
using EcommerceWebApp_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.CompilerServices;

namespace EcommerceWebApp_API.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        private ApiResponse _response;

        public ProductController(ApplicationDbContext db, IMapper mapper, IBlobService blobService)
        {
            _db = db;
            _mapper = mapper;
            _blobService = blobService;
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

        [HttpGet("{productId}")]
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

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // check if image is uploaded
                    if (productDto.ImageFile == null || productDto.ImageFile.Length == 0)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    // unique image name 
                    string imageName = $"{Guid.NewGuid()}{Path.GetExtension(productDto.ImageFile.FileName)}";

                    // check if category exists
                    var category = await _db.Categories.FirstOrDefaultAsync(c => c.CategoryId == productDto.CategoryId);
                    if (category == null)
                    {
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.IsSuccess = false;
                        _response.Errors.Add("Category not found");
                        return NotFound(_response);
                    }

                    var product = _mapper.Map<Product>(productDto);
                    product.Image = await _blobService.CreateBlob(imageName, StringConstants.storageContainer, productDto.ImageFile);
                    
                    //assign category to product
                    product.ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            CategoryId = productDto.CategoryId
                        }
                    };

                    // save product to database
                    _db.Products.Add(product);
                    await _db.SaveChangesAsync();

                    _response.Response = product;
                    _response.StatusCode = HttpStatusCode.Created;

                    // return created product
                    return CreatedAtAction("GetProduct", new { productId = product.ProductId }, _response);
                }
                else
                {
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
               
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<ApiResponse>> UpdateProduct(int productId, [FromForm] ProductUpdateDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // check if product id is valid
                    if (productId != productDto.ProductId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    // get product from database and check if it exists
                    var product = await _db.Products.FindAsync(productId);
                    if (product == null)
                    {
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.IsSuccess = false;
                        return NotFound();
                    }

                    // update product properties
                    _mapper.Map(productDto, product);
                    product.ProductId = productId;

                    // check if file was uploaded and delete old image from blob storage
                    if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
                    {
                        string imageName = $"{Guid.NewGuid()}{Path.GetExtension(productDto.ImageFile.FileName)}";
                        await _blobService.DeleteBlob(product.Image.Split('/').Last(), StringConstants.storageContainer);
                        product.Image = await _blobService.CreateBlob(imageName, StringConstants.storageContainer, productDto.ImageFile);
                    }

                    // update product in database
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<ApiResponse>> DeleteProduct(int productId)
        {
            try
            {
                //check if product id is valid
                if (productId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                // check if product exists
                var product = await _db.Products.FindAsync(productId);
                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound();
                }

                //delete product image from blob storage
                await _blobService.DeleteBlob(product.Image.Split('/').Last(), StringConstants.storageContainer);
                // wait for 2 seconds to delete image from storage before deleting the product from database 
                Thread.Sleep(2000); 

                // delete product from database
                _db.Products.Remove(product);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;
        }
    }
}