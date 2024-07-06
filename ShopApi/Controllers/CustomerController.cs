using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace ShopApi.Controllers
{
    /// <summary>
    /// Customer Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IValidatorService<Order> _validatorService;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerController(
            ICustomerService customerService, 
            IValidatorService<Order> validatorService)
        {
            _customerService = customerService;
            _validatorService = validatorService;
        }

        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>All customers paginated</returns>
        [HttpGet("getallcustomers/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItems = await _customerService.CountCustomerAsync();
                var customers = await _customerService.GetAllCustomersAsync(pageNumber, pageSize);

                var paginationData = new
                {
                    totalCount = totalItems,
                    pageSize,
                    currentPage = pageNumber,
                    totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                };

                return Ok(new { customers, paginationData });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Customer By CustomerId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A customer filter by CustomerId</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetCustomerByIdAsync(int id)
        {
            try
            {
                var result = await _customerService.GetByCustomerIdAsync(id);
                if (result == null) return BadRequest("The customer does not exists!");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateCustomerAsync(CustomerDTO customerDTO)
        {
            try
            {
                await _customerService.InsertCustomerAsync(customerDTO);
                return Ok(customerDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCustomerAsync(CustomerDTO customerDTO, int id)
        {
            try
            {
                var existing = await _customerService.GetByCustomerIdAsync(id);
                if (existing == null) return NotFound("The customer doesn't exists.");

                await _customerService.UpdateCustomerAsync(customerDTO);
                return Ok(existing);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            try
            {
                var existing = await _customerService.GetByCustomerIdAsync(id);
                if (existing == null) return NotFound("The customer doesn´t exists.");

                var relatedOrders = await _validatorService.EntityValidationsAsync(id);
                if (relatedOrders) return BadRequest("There are related customers in orders");

                await _customerService.DeleteCustomerAsync(id);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
