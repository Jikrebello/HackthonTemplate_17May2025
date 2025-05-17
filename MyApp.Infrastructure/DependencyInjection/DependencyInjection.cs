using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Application;
using MyApp.Application.Interfaces.Repositories;
using MyApp.Application.Interfaces.Services;
using MyApp.Application.Services;
using MyApp.Domain.Entities;
using MyApp.Infrastructure.Persistence;
using MyApp.Infrastructure.Repos;

namespace MyApp.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var connectionString = config.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services
            .AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // Register repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // Register services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
