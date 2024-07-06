using Domain.Entities;

namespace Repository.Repos
{
    public interface IProductRepository
    {
        Task InsertProductWithCategory(Product product, List<int> categoryIds);
        Task<IReadOnlyCollection<Product>> SearchProducts(string searchItem, int pageNumber, int pageSize);
    }
}
