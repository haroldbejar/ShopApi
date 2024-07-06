using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Repository.Repos;

namespace Service.Services
{
    public class ProductService : IProductService, IValidatorService<OrderDetail>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _repository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public ProductService(
            IMapper mapper, 
            IRepository<Product> repository, 
            IProductRepository productRepository, 
            IOrderDetailRepository orderDetailRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        
        /// <summary>
        /// Get Details by ProductId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Mapped OrderDetail</returns>
        public async Task<bool> EntityValidationsAsync(int id)
        {  
           var orderDetial = await _orderDetailRepository.GetDetailsByProductId(id);
           if (orderDetial.Count() == 0) return false;
            //return _mapper.Map<OrderDetail>(orderDetial);
           return true;
        }

        public async Task<int> CountProductsAsync()
        {
            return await _repository.CountAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new ArgumentException("Product not found");
            await _repository.DeleteAsync(id);
        }

        public async Task<IReadOnlyCollection<ProductDTO>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var products = await _repository.GetAllAsync(pageNumber, pageSize);
            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetByProductIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task InsertProductAsync(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            await _repository.InsertAsync(product);
        }

        public async Task InsertProductWithCategory(ProductDTO productDTO, List<int> categoryIds)
        {
            var product = _mapper.Map<Product>(productDTO);
            await _productRepository.InsertProductWithCategory(product, categoryIds);
        }

        public async Task UpdateProductAsync(ProductDTO productDTO)
        {
            var existingProduct = await _repository.GetByIdAsync(productDTO.ProductId);
            if (existingProduct == null) throw new ArgumentException("Product not found!");

            existingProduct.ProductCode = productDTO.ProductCode;
            existingProduct.Title = productDTO.Title;
            existingProduct.Description = productDTO.Description;
            existingProduct.Price = productDTO.Price;
            existingProduct.Stock = productDTO.Stock;
            existingProduct.ImageUrl = productDTO.ImageUrl;

            await _repository.UpdateAsync(existingProduct);
        }

        public async Task<IReadOnlyCollection<ProductDTO>> SearchProducts(string searchItem, int pageNumber, int pageSize)
        {
            var products = await _productRepository.SearchProducts(searchItem, pageNumber, pageSize);
            return _mapper.Map<List<ProductDTO>>(products);
        }

    }
}
