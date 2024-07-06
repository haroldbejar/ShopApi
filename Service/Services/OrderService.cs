using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Repository.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class OrderService : IOrderService, IValidatorService<OrderDetail>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Order> _repository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderService(
            IMapper mapper, 
            IRepository<Order> repository, 
            IOrderRepository orderRepositoty,
            IOrderDetailRepository orderDetailRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _orderRepository = orderRepositoty;
            _orderDetailRepository = orderDetailRepository;
        }

        /// <summary>
        /// Get Details by OrderId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return true if exist details</returns>
        public async Task<bool> EntityValidationsAsync(int id)
        {
            var orderDetail = await _orderDetailRepository.GetDetailsByOrderId(id);
            if (orderDetail.Count() == 0) return false;
            //return _mapper.Map<OrderDetail>(orderDetail);
            return true;
        }

        public async Task<int> CountOrdersAsync()
        {
            return await _repository.CountAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) throw new ArgumentException("Order not found");
            await _repository.DeleteAsync(id);
        }

        public async Task<IReadOnlyCollection<OrderDTO>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            var orders = await _repository.GetAllAsync(pageNumber, pageSize);
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetByOrderIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrderDTO>(order);
        }
        public async Task<IReadOnlyCollection<OrderDTO>> GetOrderByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrderByCustomerIdAsync(customerId);
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task InsertOrderAsync(OrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);
            await _repository.InsertAsync(order);
        }

        public async Task UpdateOrderAsync(OrderDTO orderDTO)
        {
            var existingOrder = await _repository.GetByIdAsync(orderDTO.OrderId);
            if (existingOrder == null) throw new ArgumentException("Order not found!");

            existingOrder.CustomerId = orderDTO.CustomerId;
            existingOrder.Description = orderDTO.Description;
            existingOrder.OrderDate = orderDTO.OrderDate;


            await _repository.UpdateAsync(existingOrder);
        }

        public async Task InsertOrderAndDetails(List<OrderDetailsViewModel> orderDetails)
        {
            await _orderRepository.InsertOrderWithDetails(orderDetails);
        }
    }
}
