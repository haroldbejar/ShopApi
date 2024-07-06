using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repos
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly WebShopContext _context;

        public UserRepository(WebShopContext context) : base (context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}