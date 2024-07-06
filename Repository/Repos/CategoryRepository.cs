using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repos
{
    public class CategoryRepository : Repository<Category>
    {
        private readonly WebShopContext _context;
        public CategoryRepository(WebShopContext context):base(context)
        {
            _context = context;
        }
        
    }
}
