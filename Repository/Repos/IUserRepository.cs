using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Repository.Repos
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserName(string userName);
    }
}