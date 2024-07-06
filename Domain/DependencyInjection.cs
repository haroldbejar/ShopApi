using Domain.Data;
using Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services, string conString)
        {
            services.AddDbContext<WebShopContext>(options =>
            {
                options.UseSqlServer(conString);
            });

            return services;
        }

        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(ProductProfile),
                typeof(CategoryProfile),
                typeof(CustomerProfile),
                typeof(OrderProfile),
                typeof(ProductCategoryProfile),
                typeof(OrderDetailProfile));

            return services;
        }
    }
}
