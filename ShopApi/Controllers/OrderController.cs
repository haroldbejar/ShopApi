using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace ShopApi.Controllers
{
    /// <summary>
    /// Order Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidatorService<OrderDetail> _validatorService;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderController(IOrderService orderService, IValidatorService<OrderDetail> validatorService)
        {
            _orderService = orderService;
            _validatorService = validatorService;   
        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>All Orders paginated</returns>
        [HttpGet("getallorders/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItems = await _orderService.CountOrdersAsync();
                var customers = await _orderService.GetAllOrdersAsync(pageNumber, pageSize);

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
        /// Get Order by orderId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A order filter by orderId</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetOrderByIdAsync(int id)
        {
            try
            {
                var result = await _orderService.GetByOrderIdAsync(id);
                if (result == null) return BadRequest("The order does not exists!");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Orders by customerId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Orders filter by customerId</returns>
        [HttpGet("getorderbycustomer/{id:int}")]
        public async Task<ActionResult> GetOrderByCustumerIdAsync(int id)
        {
            try
            {
                var result = await _orderService.GetOrderByCustomerIdAsync(id);
                if (result == null) return BadRequest("There aren´t orders for this customer.");

                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateOrderAsync(OrderDTO orderDTO)
        {
            try
            {
                await _orderService.InsertOrderAsync(orderDTO);
                return Ok(orderDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create Order and detials 
        /// </summary>
        /// <param name="orderDetials"></param>
        /// <returns></returns>
        [HttpPost("createdetails")]
        public async Task<ActionResult> CreateOrderDetailAsync(List<OrderDetailsViewModel> orderDetials)
        {
            try
            {
                await _orderService.InsertOrderAndDetails(orderDetials);
                return Ok(orderDetials);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateOrderAsync(OrderDTO orderDTO, int id)
        {
            try
            {
                var existing = await _orderService.GetByOrderIdAsync(id);
                if (existing == null) return NotFound("The order doesn't exists.");

                await _orderService.UpdateOrderAsync(orderDTO);
                return Ok(existing);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                var existing = await _orderService.GetByOrderIdAsync(id);
                if (existing == null) return NotFound("The order doesn´t exists.");

                var relatedDetials = await _validatorService.EntityValidationsAsync(id);
                if (relatedDetials) return BadRequest("There are details related!");

                await _orderService.DeleteOrderAsync(id);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
