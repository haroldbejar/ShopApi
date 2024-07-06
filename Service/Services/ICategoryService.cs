using Domain.DTOs;

namespace Service.Services
{
    public interface ICategoryService
    {
        Task<int> CountCategoryAsync();
        Task DeleteCategoryAsync(int id);
        Task<IReadOnlyCollection<CategoryDTO>> GetAllCategoriesAsync(int pageNumber, int pageSize);
        Task<CategoryDTO> GetByCategoryIdAsync(int id);
        Task InsertCategotyAsync(CategoryDTO categoryDTO);
        Task UpdateCatyegoryAsync(CategoryDTO categoryDTO);
    }
}
