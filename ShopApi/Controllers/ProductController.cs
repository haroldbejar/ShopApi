using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace ShopApi.Controllers
{
    /// <summary>
    /// Product Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidatorService<OrderDetail> _validatorService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="validatorService"></param>
        public ProductController(IProductService productService, IValidatorService<OrderDetail> validatorService)
        {
            _productService = productService;
            _validatorService = validatorService;
        }

        /// <summary>
        /// Get All Products
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>All products paginated</returns>
        [HttpGet("getallproducts/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItems = await _productService.CountProductsAsync();
                var products = await _productService.GetAllProductsAsync(pageNumber, pageSize);

                var paginationData = new
                {
                    totalCount = totalItems,
                    pageSize,
                    currentPage = pageNumber,
                    totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                };

                return Ok(new {products, paginationData});
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Product By ProductId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A product filter by ProductId</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProductByIdAsync(int id)
        {
            try
            {
                var result = await _productService.GetByProductIdAsync(id);
                if (result == null) return BadRequest("The product does not exists!");

                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create Product 
        /// </summary>
        /// <param name="productDTO"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateProductAsync([FromBody] ProductDTO productDTO, [FromQuery] List<int> categoryIds)
        {
            try
            {
                await _productService.InsertProductWithCategory(productDTO, categoryIds);
                return Ok(productDTO);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="productDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProductAsync(ProductDTO productDTO, int id)
        {
            try
            {
                var existing = await _productService.GetByProductIdAsync(id);
                if (existing == null) return NotFound("The product doesn't exists.");

                await _productService.UpdateProductAsync(productDTO);
                return Ok(existing);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            try
            {
                var existing = await _productService.GetByProductIdAsync(id);
                if (existing == null) return NotFound("The product doesn´t exists.");

                var relatedDetails = await _validatorService.EntityValidationsAsync(id);
                if (relatedDetails) return BadRequest("There are related products in orders");

                await _productService.DeleteProductAsync(id);
                return Ok();

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Searche products
        /// </summary>
        /// <param name="searchItem"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Products finded</returns>
        [HttpGet("search/{searchItem}/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult> SearchProducts(string searchItem, int pageNumber, int pageSize)
        {
            try
            {
                var searchedProduct = await _productService.SearchProducts(searchItem, pageNumber, pageSize);
                if (searchedProduct == null) return NotFound("There are not products related with this search!");
                return Ok(searchedProduct);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
