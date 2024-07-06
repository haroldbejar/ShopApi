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
    public class CustomerService : ICustomerService, IValidatorService<Order>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Customer> _repository;
        private readonly IOrderRepository _orderRepository;

        public CustomerService(IMapper mapper, IRepository<Customer> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// Get Orders by CustomerId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return true is exist related Orders</returns>
        public async Task<bool> EntityValidationsAsync(int id)
        {   
            var order = await _orderRepository.GetOrderByCustomerIdAsync(id);
            if (order.Count() == 0) return false;
            //return _mapper.Map<Order>(order);
            return true;
        }

        public async Task<int> CountCustomerAsync()
        {
            return await _repository.CountAsync();
        }
        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null) throw new ArgumentException("Customer not found");
            await _repository.DeleteAsync(id);
        }

        public async Task<IReadOnlyCollection<CustomerDTO>> GetAllCustomersAsync(int pageNumber, int pageSize)
        {
            var customers = await _repository.GetAllAsync(pageNumber, pageSize);
            return _mapper.Map<List<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetByCustomerIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task InsertCustomerAsync(CustomerDTO customerDTO)
        {
            var customer = _mapper.Map<Customer>(customerDTO);
            await _repository.InsertAsync(customer);
        }

        public async Task UpdateCustomerAsync(CustomerDTO customerDTO)
        {
            var existingCustomer = await _repository.GetByIdAsync(customerDTO.CustomerId);
            if (existingCustomer == null) throw new ArgumentException("Customer not found!");

            existingCustomer.Name = customerDTO.Name;
            existingCustomer.Email = customerDTO.Email;
           

            await _repository.UpdateAsync(existingCustomer);
        }
    }
}
