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

        public CategoryController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadOnlyDto>>> GetCategories()
        {
            if (_db.Categories == null)
            {
                return NotFound("No categories found");
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
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Invalid category id");
                return BadRequest(_response);
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
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                // Return category and OK response
                _response.Response = category;
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

                    // return the created category
                    return CreatedAtAction("GetCategory", new { categoryId = category.CategoryId }, _response);
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
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.Errors.Add("Invalid category id");
                        return BadRequest(_response);
                    }

                    // Fetch the category from the database and check if it exists
                    var category = await _db.Categories.FindAsync(categoryId);
                    if (category == null)
                    {
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

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(int categoryId)
        {
            try
            {
                // check if category id is valid
                if (categoryId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.Errors.Add("Invalid category id");
                    return BadRequest(_response);
                }

                // Fetch the category from the database and check if it exists
                var category = await _db.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound();
                }

                // delete the category from the database
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
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
