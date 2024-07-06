using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IValidatorService<OrderDetail>, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IValidatorService<Order>, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IValidatorService<OrderDetail>, OrderService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IValidatorService<ProductCategory>, CategoryService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
