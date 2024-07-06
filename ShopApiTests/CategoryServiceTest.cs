using AutoMapper;
using Castle.Core.Resource;
using Domain.DTOs;
using Domain.Entities;
using NSubstitute;
using Repository.Repos;
using Service.Services;

namespace ShopApiTests
{
    public class CategoryServiceTest
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _repository;
        private readonly ICategoryService _categoryService;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public CategoryServiceTest()
        {
            _mapper = Substitute.For<IMapper>();
            _repository = Substitute.For<IRepository<Category>>();
            _productCategoryRepository = Substitute.For<IProductCategoryRepository>();
            _categoryService = new CategoryService(_mapper, _repository, _productCategoryRepository);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsMappedCategoriesDTO()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Name = "Category 1"},
                new Category { Name = "Category 2"},
            };

            var categoriesDTO = new List<CategoryDTO>
            {
                 new CategoryDTO { Name = "Category 1"},
                new CategoryDTO { Name = "Category 2"},
            };

            _repository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(categories);
            _mapper.Map<List<CategoryDTO>>(categories).Returns(categoriesDTO);

            // Act
            var result = await _categoryService.GetAllCategoriesAsync(1, 10);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async Task GetByCategoryIdAsync_ReturnsMappedCategoryDTO()
        {
            // Arrange
            int categoryId = 1;
            var category = new Category { Name = "Category 1"};
            var categoryDTO = new CategoryDTO { Name = "Customer 1" };

            _repository.GetByIdAsync(categoryId).Returns(category);
            _mapper.Map<CategoryDTO>(category).Returns(categoryDTO);

            // Act
            var result = await _categoryService.GetByCategoryIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task InsertCategoryAsync_ThrowsException_WhenMapperFails()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Name = "Category 1" };
            _mapper.Map<Category>(categoryDTO).Returns(x => { throw new AutoMapperMappingException("Mapping failed"); });

            // Act & Assert
            await Assert.ThrowsAsync<AutoMapperMappingException>(() => _categoryService.InsertCategotyAsync(categoryDTO));

        }
    }
}
