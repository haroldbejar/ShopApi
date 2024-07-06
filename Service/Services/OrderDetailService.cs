using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Repository.Repos;

namespace Service.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<OrderDetail> _repository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(
            IMapper mapper, 
            IRepository<OrderDetail> repository, 
            IOrderDetailRepository orderDetailRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _orderDetailRepository = orderDetailRepository;
        }
       
        public async Task<int> CountOrderDetailsAsync()
        {
            return await _repository.CountAsync();
        }
        public async Task DeleteOrderDetialAsync(int id)
        {
            var orderDetail = await _repository.GetByIdAsync(id);
            if (orderDetail == null) throw new ArgumentException("OrderDetial not found");
            await _repository.DeleteAsync(id);
        }

        public async Task<IReadOnlyCollection<OrderDetailDTO>> GetAlOrderDetialsAsync(int pageNumber, int pageSize)
        {
            var orderDetail = await _repository.GetAllAsync(pageNumber, pageSize);
            return _mapper.Map<List<OrderDetailDTO>>(orderDetail);
        }

        public async Task<OrderDetailDTO> GetByOrderDetailIdAsync(int id)
        {
            var orderDetail = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrderDetailDTO>(orderDetail);
        }

        public async Task InsertOrderDetialAsync(OrderDetailDTO orderDetailDTO)
        {
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
            await _repository.InsertAsync(orderDetail);
        }
        public async Task UpdateOrderDetialAsync(OrderDetailDTO orderDetailDTO)
        {
            var existingOrderDetail = await _repository.GetByIdAsync(orderDetailDTO.OrderDetailId);
            if (existingOrderDetail == null) throw new ArgumentException("Order detial not found!");

            existingOrderDetail.OrderId = orderDetailDTO.OrderId;
            existingOrderDetail.ProductId = orderDetailDTO.ProductId;
            existingOrderDetail.Quantity = orderDetailDTO.Quantity;
            existingOrderDetail.Price = orderDetailDTO.Price;

            await _repository.UpdateAsync(existingOrderDetail);
        }

        public async Task<IReadOnlyCollection<OrderDetailDTO>> GetDetailsByOrderId(int orderId)
        {
            var details = await _orderDetailRepository.GetDetailsByOrderId(orderId);
            return _mapper.Map<List<OrderDetailDTO>>(details);
        }

        public async Task<IReadOnlyCollection<OrderDetailDTO>> GetDetailsByProductId(int productId)
        {
            var details = await _orderDetailRepository.GetDetailsByProductId(productId);
            return _mapper.Map<List<OrderDetailDTO>>(details);
        }

    }
}
