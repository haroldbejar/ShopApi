using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repos
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly WebShopContext _context;
        public ProductRepository(WebShopContext context): base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Product>> SearchProducts(string searchItem, int pageNumber, int pageSize)
        {
            var product = await _context.Products
                    .Where(x => x.Title.Contains(searchItem))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return product;
                    
        }

        public async Task InsertProductWithCategory(Product product, List<int> categoryIds)
        {
            _context.Products.Add(product);

            // Create and add relations of ProductCategory
            foreach (var categoryId in categoryIds) 
            {
                var productCategory = new ProductCategory
                {
                    Product = product,
                    CategoryId = categoryId
                };
               await _context.ProductCategories.AddAsync(productCategory);
            }

            await _context.SaveChangesAsync();
        }

        
    }
}
