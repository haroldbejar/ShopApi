using AutoMapper;
using Domain.Entities;
using Repository.Repos;
using Service.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs;

namespace ShopApiTests
{
    public class ProductServiceTest
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _repository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public ProductServiceTest()
        {
            _mapper = Substitute.For<IMapper>();
            _repository = Substitute.For<IRepository<Product>>();
            _productRepository = Substitute.For<IProductRepository>();
            _orderDetailRepository = Substitute.For<IOrderDetailRepository>();
            _productService = new ProductService(
                _mapper, 
                _repository, 
                _productRepository, 
                _orderDetailRepository);
        }

        [Fact]
        public async Task GetAllProductAsync_ReturnsMappedProductDTO()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1" },
                new Product { ProductId = 2, ProductCode = "Code 2", Title = "Product 2", Description = "This is a product 2", Price = 300, Stock = 30, ImageUrl = "image2" },
            };

            var productDTO = new List<ProductDTO>
            {
                new ProductDTO { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1"},
                new ProductDTO { ProductId = 2, ProductCode = "Code 2", Title = "Product 2", Description = "This is a product 2", Price = 300, Stock = 30, ImageUrl = "image2"},
            };

            _repository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(products);
            _mapper.Map<List<ProductDTO>>(products).Returns(productDTO);

            // Act
            var result = await _productService.GetAllProductsAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

        }

        [Fact]
        public async Task GetByProductIdAsync_ReturnsMappedProductDTO()
        {
            // Arrange
            int productId = 1;
            var product = new Product { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1" };
            var productDTO = new ProductDTO { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1" };

            _repository.GetByIdAsync(productId).Returns(product);
            _mapper.Map<ProductDTO>(product).Returns(productDTO);

            // Act
            var result = await _productService.GetByProductIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.ProductId);
        }

        [Fact]
        public async Task GetByProductIdAsync_ReturnsNull_WhenProductNotFound()
        {
            // Arrange
            int productId = 1;
            _repository.GetByIdAsync(productId).Returns(Task.FromResult<Product>(null));

            // Act
            var result = await _productService.GetByProductIdAsync(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task InsertProductAsync_CallsRepositoryInsertAsync_WithMappedProduct()
        {
            // Arrange
            var productDTO = new ProductDTO { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1" };
            var product = new Product { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1" };

            _mapper.Map<Product>(productDTO).Returns(product);

            // Act
            await _productService.InsertProductAsync(productDTO);

            // Assert
            await _repository.Received(1).InsertAsync(product);
        }

        [Fact]
        public async Task InsertProductAsync_ThrowsException_WhenMapperFails()
        {
            // Arrange
            var productDTO = new ProductDTO { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1" };

            _mapper.Map<Product>(productDTO).Returns(x => { throw new AutoMapperMappingException("Mapping failed"); });

            // Act & Assert
            await Assert.ThrowsAsync<AutoMapperMappingException>(() => _productService.InsertProductAsync(productDTO));
        }

        [Fact]
        public async Task SearchProducts_ReturnsMappedProductDTOs()
        {
            // Arrange
            string searchItem = "test";
            int pageNumber = 1;
            int pageSize = 10;

            var products = new List<Product>
            {
                new Product { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1" },
                new Product { ProductId = 2, ProductCode = "Code 2", Title = "Product 2", Description = "This is a product 2", Price = 300, Stock = 30, ImageUrl = "image2" },
            };

            var productDTO = new List<ProductDTO>
            {
                new ProductDTO { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1"},
                new ProductDTO { ProductId = 2, ProductCode = "Code 2", Title = "Product 2", Description = "This is a product 2", Price = 300, Stock = 30, ImageUrl = "image2"},
            };

            _productRepository.SearchProducts(searchItem, pageNumber, pageSize).Returns(Task.FromResult((IReadOnlyCollection<Product>)products));
            _mapper.Map<List<ProductDTO>>(products).Returns(productDTO);

            // Act
            var result = await _productService.SearchProducts(searchItem, pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task DeleteProductAsync_DeletesProduct_WhenProductExists()
        {
            // Arrange
            int productId = 1;
            var existingProduct = new Product { ProductId = 1, ProductCode = "Code 1", Title = "Product 1", Description = "This is a product 1", Price = 200, Stock = 20, ImageUrl = "image1" };

            _repository.GetByIdAsync(productId).Returns(Task.FromResult(existingProduct));

            // Act
            await _productService.DeleteProductAsync(productId);

            // Assert
            await _repository.Received(1).DeleteAsync(productId);
        }

        [Fact]
        public async Task DeleteProductAsync_ThrowsException_WhenProductDoesNotExist()
        {
            // Arrange
            int productId = 1;
            _repository.GetByIdAsync(productId).Returns(Task.FromResult<Product>(null));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.DeleteProductAsync(productId));
            Assert.Equal("Product not found", exception.Message);
        }


    }
}
