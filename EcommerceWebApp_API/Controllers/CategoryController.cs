using AutoMapper;
using EcommerceWebApp_API.Data;
using EcommerceWebApp_API.Models.Category;
using EcommerceWebApp_API.Models.Product;
using EcommerceWebApp_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EcommerceWebApp_API.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ApplicationDbContext db, IMapper mapper, ILogger<CategoryController> logger)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadOnlyDto>>> GetCategories()
        {
            if (_db.Categories == null)
            {
                _logger.LogWarning($"Data not found in {nameof(GetCategories)}");
                return NotFound();
            }

            try
            {
                // Fetch all categories from the database and return them
                var categories = await _db.Categories
                    .Include(pc => pc.ProductCategories)
                    .ThenInclude(p => p.Product)
                    .ToListAsync();
                _response.StatusCode = HttpStatusCode.OK;

                var categoriesDto = _mapper.Map<IEnumerable<CategoryReadOnlyDto>>(categories);

                _response.Response = categoriesDto;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: GET in {nameof(GetCategories)}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryReadOnlyDto>> GetCategory(int categoryId)
        {
            // check if the category is valid
            if (categoryId == 0)
            {
                _logger.LogWarning($"Invalid category id in {nameof(GetCategory)} - ID: {categoryId}");
                return BadRequest();
            }

            try
            {
                // Fetch the category from the database and return it
                var category = await _db.Categories
                    .Include(pc => pc.ProductCategories)
                    .ThenInclude(p => p.Product)
                    .Where(c => c.CategoryId == categoryId)
                    .FirstOrDefaultAsync();

                // check if the category exists
                if (category == null)
                {
                    // Return not found response
                    _logger.LogWarning($"No data found in {nameof(GetCategory)} - ID: {categoryId}");
                    return NotFound();
                }

                // Return category and OK response
                _response.Response = category;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: GET in {nameof(GetCategory)}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CategoryCreateDto categoryDto)
        {
            try 
            {
                if (ModelState.IsValid)
                {
                    var category = _mapper.Map<Category>(categoryDto);

                    // Add the category to the database
                    _db.Categories.Add(category);
                    await _db.SaveChangesAsync();

                    _response.Response = category;
                    _response.StatusCode = HttpStatusCode.Created;
                    _logger.LogInformation($"Data created in {nameof(CreateCategory)} - ID: {category.CategoryId}");

                    // return the created category
                    return CreatedAtAction("GetCategory", new { categoryId = category.CategoryId }, _response);
                }
                else
                {
                    _logger.LogWarning($"Invalid model state in {nameof(CreateCategory)}");
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: POST in {nameof(CreateCategory)}");
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpPut("{categoryId}")]
        public async Task<ActionResult<ApiResponse>> UpdateCategory(int categoryId, [FromForm] CategoryUpdateDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    // check if category id is valid
                    if (categoryId != categoryDto.CategoryId)
                    {
                        _logger.LogWarning($"Invalid category id in {nameof(UpdateCategory)} - ID: {categoryId}");
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.Errors.Add("Invalid category id");
                        return BadRequest(_response);
                    }

                    // Fetch the category from the database and check if it exists
                    var category = await _db.Categories.FindAsync(categoryId);
                    if (category == null)
                    {
                        _logger.LogWarning($"No data found in {nameof(UpdateCategory)} - ID: {categoryId}");
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.IsSuccess = false;
                        return NotFound();
                    }

                    // update the category properties
                    _mapper.Map(categoryDto, category);
                    category.CategoryId = categoryId;

                    // update the category in the database
                    _db.Categories.Update(category);
                    await _db.SaveChangesAsync();
                    _logger.LogInformation($"Data updated in {nameof(UpdateCategory)} - ID: {categoryId}");
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
                _logger.LogError(ex, $"Error: PUT in {nameof(UpdateCategory)}");
                _response.IsSuccess = false;
                _response.Errors = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(int categoryId)
        {
            try
            {
                // check if category id is valid
                if (categoryId == 0)
                {
                    _logger.LogWarning($"Invalid category id in {nameof(DeleteCategory)} - ID: {categoryId}");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.Errors.Add("Invalid category id");
                    return BadRequest(_response);
                }

                // Fetch the category from the database and check if it exists
                var category = await _db.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    _logger.LogWarning($"No data found in {nameof(DeleteCategory)} - ID: {categoryId}");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound();
                }

                // delete the category from the database
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
                _response.StatusCode = HttpStatusCode.NoContent;
                _logger.LogInformation($"Data deleted in {nameof(DeleteCategory)} - ID: {categoryId}");
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: DELETE in {nameof(DeleteCategory)}");
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
