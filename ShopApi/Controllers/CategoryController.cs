using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace ShopApi.Controllers
{
    /// <summary>
    /// Category Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidatorService<ProductCategory> _validatorService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="categoryService"></param>
        /// <param name="validatorService"></param>
        public CategoryController(ICategoryService categoryService, IValidatorService<ProductCategory> validatorService)
        {
            _categoryService = categoryService;
            _validatorService = validatorService;

        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>All categories paginated</returns>
        [HttpGet("getallcategories/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItems = await _categoryService.CountCategoryAsync();
                var categories = await _categoryService.GetAllCategoriesAsync(pageNumber, pageSize);

                var paginationData = new
                {
                    totalCount = totalItems,
                    pageSize,
                    currentPage = pageNumber,
                    totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                };

                return Ok(new { categories, paginationData });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Category by categoryId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A category filter by cagtegoryId</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetCategoryByIdAsync(int id)
        {
            try
            {
                var result = await _categoryService.GetByCategoryIdAsync(id);
                if (result == null) return BadRequest("The category does not exists!");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create Categoy
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateCategoryAsync(CategoryDTO categoryDTO)
        {
            try
            {
                await _categoryService.InsertCategotyAsync(categoryDTO);
                return Ok(categoryDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategoryAsync(CategoryDTO categoryDTO, int id)
        {
            try
            {
                var existing = await _categoryService.GetByCategoryIdAsync(id);
                if (existing == null) return NotFound("The category doesn't exists.");

                await _categoryService.UpdateCatyegoryAsync(categoryDTO);
                return Ok(existing);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategoryAsync(int id)
        {
            try
            {
                var existing = await _categoryService.GetByCategoryIdAsync(id);
                if (existing == null) return NotFound("The category doesn´t exists.");

                var relatedCategory = await _validatorService.EntityValidationsAsync(id);
                if (relatedCategory) return BadRequest("There are products related!");

                await _categoryService.DeleteCategoryAsync(id);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
