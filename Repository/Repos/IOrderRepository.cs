using Domain.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    public interface IOrderRepository
    {
        Task<IReadOnlyCollection<Order>> GetOrderByCustomerIdAsync(int customerId);
        Task InsertOrderWithDetails(List<OrderDetailsViewModel> orderViewModels);
    }
}
