using Domain.Data;
using Domain.Entities;

namespace Repository.Repos
{
    public class CustomerRepository : Repository<Customer>
    {
        private readonly WebShopContext _context;
        public CustomerRepository(WebShopContext context) :base(context)
        {
            _context = context;
        }
    }
}
