using Domain.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    public interface IProductCategoryRepository
    {
        Task<IReadOnlyCollection<ProductCategoryDTO>> GetByCategoryId(int categoryId, int pageNumber, int pageSize);
        Task<IReadOnlyCollection<ProductCategoryDTO>> GetByProductId(int productId, int pageNumber, int pageSize);
    }
}
