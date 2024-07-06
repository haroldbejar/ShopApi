using Domain.Entities;

namespace Repository.Repos
{
    public interface IOrderDetailRepository
    {
        Task<IReadOnlyCollection<OrderDetail>> GetDetailsByOrderId(int orderId);
        Task<IReadOnlyCollection<OrderDetail>> GetDetailsByProductId(int productId);
    }
}
