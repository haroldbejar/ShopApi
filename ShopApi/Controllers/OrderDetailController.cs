using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System.Runtime.CompilerServices;

namespace ShopApi.Controllers
{
    /// <summary>
    /// OrderDetail Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderDetailService"></param>
        /// <param name="validatorService"></param>
        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        /// <summary>
        /// Get All Order Details
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>All the detail paginated</returns>
        [HttpGet("getallorderdetail/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult> GetAllOrderDetailsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItems = await _orderDetailService.CountOrderDetailsAsync();
                var orderDetails = await _orderDetailService.GetAlOrderDetialsAsync(pageNumber,pageSize);

                var paginationData = new
                {
                    totalCount = totalItems,
                    pageSize,
                    currentPage = pageNumber,
                    totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                };

                return Ok(new { orderDetails, paginationData });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Order Detail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Get a specific detail filter by id</returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetOrderDetailByIdAsync(int id)
        {
            try
            {
                var result = await _orderDetailService.GetByOrderDetailIdAsync(id);
                if (result == null) throw new ArgumentException("The detail does not exist!");
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Order Detail
        /// </summary>
        /// <param name="orderDetailDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateOrderDetailById(OrderDetailDTO orderDetailDTO, int id)
        {
            try
            {
                var existing = await _orderDetailService.GetByOrderDetailIdAsync(id);
                if (existing == null) throw new ArgumentException("The detail does not exist!");

                await _orderDetailService.UpdateOrderDetialAsync(orderDetailDTO);
                return Ok(existing);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Order Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteOrderDetailAsync(int id)
        {
            try
            {
                var existing = await _orderDetailService.GetByOrderDetailIdAsync(id);
                if (existing == null) throw new ArgumentException("The detail does not exist!");

                await _orderDetailService.DeleteOrderDetialAsync(id);
                return Ok();

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
