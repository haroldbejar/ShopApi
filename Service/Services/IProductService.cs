using Domain.DTOs;

namespace Service.Services
{
    public interface IProductService
    {
        Task<int> CountProductsAsync();
        Task DeleteProductAsync(int id);
        Task<IReadOnlyCollection<ProductDTO>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<ProductDTO> GetByProductIdAsync(int id);
        Task InsertProductAsync(ProductDTO productDTO);
        Task UpdateProductAsync(ProductDTO productDTO);
        Task<IReadOnlyCollection<ProductDTO>> SearchProducts(string searchItem, int pageNumber, int pageSize);
        Task InsertProductWithCategory(ProductDTO productDTO, List<int> categoryIds);
    }
}
