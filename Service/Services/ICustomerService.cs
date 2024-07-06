using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface ICustomerService
    {
        Task<int> CountCustomerAsync();
        Task DeleteCustomerAsync(int id);
        Task<IReadOnlyCollection<CustomerDTO>> GetAllCustomersAsync(int pageNumber, int pageSize);
        Task<CustomerDTO> GetByCustomerIdAsync(int id);
        Task InsertCustomerAsync(CustomerDTO customerDTO);
        Task UpdateCustomerAsync(CustomerDTO customerDTO);
    }
}
