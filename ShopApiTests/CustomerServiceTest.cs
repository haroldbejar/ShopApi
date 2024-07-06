using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using NSubstitute;
using Repository.Repos;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApiTests
{
    public class CustomerServiceTest
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Customer> _repository;
        private readonly ICustomerService _customerService;

        public CustomerServiceTest()
        {
            _mapper = Substitute.For<IMapper>();
            _repository = Substitute.For<IRepository<Customer>>();
            _customerService = new CustomerService(_mapper, _repository);
        }

        [Fact]
        public async Task GetAllCustomerAsync_ReturnsMappedCustomerDTO()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1", Email = "customer1@example.com" },
                new Customer { Name = "Customer 2", Email = "customer2@example.com" },
            };
            var customerDtos = new List<CustomerDTO>
            {
                new CustomerDTO { Name = "Customer 1", Email = "customer1@example.com" },
                new CustomerDTO { Name = "Customer 2", Email = "customer2@example.com" },
            };

            _repository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(customers);
            _mapper.Map<List<CustomerDTO>>(customers).Returns(customerDtos);

            // Act
            var result = await _customerService.GetAllCustomersAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByCustomerIdAsync_ReturnsMappedCustomerDTO()
        {
            // Arrange
            int customerId = 1;
            var customer = new Customer { Name = "Customer 1", Email = "customer1@example.com"  };
            var customerDTO = new CustomerDTO { Name = "Customer 1", Email = "customer1@example.com" };
            
            _repository.GetByIdAsync(customerId).Returns(customer);
            _mapper.Map<CustomerDTO>(customer).Returns(customerDTO);

            // Act
            var result = await _customerService.GetByCustomerIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task InsertCustomerAsync_ThrowsException_WhenMapperFails()
        {
            // Arrange
            var customerDTO = new CustomerDTO { Name = "Customer 1", Email = "customer1@example.com" };

            _mapper.Map<Customer>(customerDTO).Returns(x => { throw new AutoMapperMappingException("Mapping failed"); });

            // Act & Assert
            await Assert.ThrowsAsync<AutoMapperMappingException>(() => _customerService.InsertCustomerAsync(customerDTO));
        }

        [Fact]
        public async Task DeleteCustomerAsync_DeletesCustomer_WhenCustomerExists()
        {
            // Arrange
            int customerId = 1;
            var existingCustomer = new Customer { Name = "Customer 1", Email = "customer1@example.com" };

            _repository.GetByIdAsync(customerId).Returns(Task.FromResult(existingCustomer));

            // Act
            await _customerService.DeleteCustomerAsync(customerId);

            // Assert
            await _repository.Received(1).DeleteAsync(customerId);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ThrowsException_WhenCustomerDoesNotExist()
        {
            // Arrange
            int customerId = 1;
            _repository.GetByIdAsync(customerId).Returns(Task.FromResult<Customer>(null));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _customerService.DeleteCustomerAsync(customerId));
            Assert.Equal("Customer not found", exception.Message);
        }
    }
}
