using Domain.Data;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repos
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly WebShopContext _context;
        public OrderRepository(WebShopContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Order>> GetOrderByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderDetails)
                .ToListAsync();
        }

        public async Task InsertOrderWithDetails(List<OrderDetailsViewModel> orderViewModels)
        {
            if (orderViewModels == null || orderViewModels.Count == 0)
            {
                throw new ArgumentException("Invalid order data");
            }

            // Insert order details
            await _context.Orders.AddRangeAsync(orderViewModels.Select(o => o.Order));
            await _context.SaveChangesAsync();

            // Get all details of all orders in a single list
            var allOrderDetails = orderViewModels
                .SelectMany(o => o.OrderDetails)
                .ToList();

            // Assign OrderId to each order detail based on its position in the list
            for (int i = 0; i < orderViewModels.Count; i++)
            {
                int orderId = orderViewModels[i].Order.OrderId;
                foreach (var detail in orderViewModels[i].OrderDetails)
                {
                    detail.OrderId = orderId;
                }
            }

            // Insert order details
            await _context.OrderDetails.AddRangeAsync(allOrderDetails);
            await _context.SaveChangesAsync();
        }
    }
}
