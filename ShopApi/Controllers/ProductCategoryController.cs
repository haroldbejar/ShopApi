using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Service.Services;

namespace ShopApi.Controllers
{
    /// <summary>
    /// ProductCategory Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productCategoryService"></param>
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        /// <summary>
        /// Get Products by CategoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>A list of products paginated find by categoryId</returns>
        [HttpGet("byCategory/{categoryId:int}/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult> GetProductsByCategoryId(int categoryId, int pageNumber, int pageSize)
        {
            try
            {
                var totalItem = await _productCategoryService.CountProductCategoryAsync();
                var productsCategory = await _productCategoryService.GetByCategoryId(categoryId, pageNumber, pageSize);

                var paginationData = new
                {
                    totalCount = totalItem,
                    pageSize,
                    currentPage = pageNumber,
                    totalpage = (int)Math.Ceiling((double)totalItem / pageSize),
                };
                
                return Ok(new { productsCategory, paginationData });
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
