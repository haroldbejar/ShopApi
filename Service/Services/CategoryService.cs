using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Repository.Repos;

namespace Service.Services
{
    public class CategoryService : ICategoryService, IValidatorService<ProductCategory>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _repository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public CategoryService(
            IMapper mapper, 
            IRepository<Category> repository, 
            IProductCategoryRepository productCategoryRepository )
        {
            _mapper = mapper;
            _repository = repository;
            _productCategoryRepository = productCategoryRepository;
        }

        /// <summary>
        /// Get Categories by productId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return true is exist related documents</returns>
        public async Task<bool> EntityValidationsAsync(int id)
        {
            int pageNumber = 1;
            int pageSize = 10;
            var productCategory = await _productCategoryRepository.GetByCategoryId(id, pageNumber, pageSize);
            if (productCategory.Count > 0) return true;
            return false;
        }

    public async Task<int> CountCategoryAsync()
        {
            return await _repository.CountAsync();
        } 

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) throw new ArgumentException("Category not found");
            await _repository.DeleteAsync(id);
        }

        public async Task<IReadOnlyCollection<CategoryDTO>> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            var categories = await _repository.GetAllAsync(pageNumber, pageSize);
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetByCategoryIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task InsertCategotyAsync(CategoryDTO categoryDTO)
        {
            var catetory = _mapper.Map<Category>(categoryDTO);
            await _repository.InsertAsync(catetory);
        }

        public async Task UpdateCatyegoryAsync(CategoryDTO categoryDTO)
        {
            var existingCategory = await _repository.GetByIdAsync(categoryDTO.CategoryId);
            if (existingCategory == null) throw new ArgumentException("Category doesn´t exist!");
            existingCategory.Name = categoryDTO.Name;
            await _repository.UpdateAsync(existingCategory);
        }
    }
}
