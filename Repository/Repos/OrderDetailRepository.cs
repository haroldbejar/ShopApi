using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repos
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly WebShopContext _context;
        public OrderDetailRepository(WebShopContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<OrderDetail>> GetDetailsByOrderId(int orderId)
        {
            return await _context.OrderDetails.Where(d => d.OrderId == orderId).ToListAsync();
        }

        public async Task<IReadOnlyCollection<OrderDetail>> GetDetailsByProductId(int productId)
        {
            return await _context.OrderDetails.Where(d => d.ProductId == productId).ToListAsync();
        }
    }
}
