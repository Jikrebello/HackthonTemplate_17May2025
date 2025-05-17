using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Common.Constants;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        var roles = new[] { "User", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
    
    public static async Task SeedDataAsync(AppDbContext context, UserManager<AppUser> userManager)
    {
        // Check if the database is properly migrated
        bool isMigrated = false;
        try
        {
            // Try to access the UserPermissions table
            isMigrated = await context.Database.CanConnectAsync();
            
            // Only seed if no data exists
            if (await context.Categories.AnyAsync() || await context.Products.AnyAsync() || await userManager.Users.AnyAsync())
            {
                return;
            }
        }
        catch
        {
            // If there's an error, the database might not be migrated yet
            return;
        }
        
        // Create default categories
        var categories = new List<Category>
        {
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Electronics",
                Description = "Electronic devices and accessories",
                isDefault = true
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Clothing",
                Description = "Apparel and fashion items",
                isDefault = false
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Home & Garden",
                Description = "Items for home decoration and gardening",
                isDefault = false
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Books",
                Description = "Books, e-books, and publications",
                isDefault = false
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Sports & Outdoors",
                Description = "Sporting goods and outdoor equipment",
                isDefault = false
            }
        };
        
        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
        
        // Create products
        var products = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Smartphone X",
                Description = "Latest model with advanced features",
                Price = 899.99m,
                Quantity = 50,
                CategoryId = categories[0].Id // Electronics
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Laptop Pro",
                Description = "High-performance laptop for professionals",
                Price = 1299.99m,
                Quantity = 30,
                CategoryId = categories[0].Id // Electronics
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Designer T-Shirt",
                Description = "Premium cotton t-shirt with unique design",
                Price = 49.99m,
                Quantity = 100,
                CategoryId = categories[1].Id // Clothing
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Gardening Tool Set",
                Description = "Complete set of essential gardening tools",
                Price = 79.99m,
                Quantity = 25,
                CategoryId = categories[2].Id // Home & Garden
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Bestseller Novel",
                Description = "Award-winning fiction novel",
                Price = 24.99m,
                Quantity = 75,
                CategoryId = categories[3].Id // Books
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Mountain Bike",
                Description = "All-terrain mountain bike for outdoor adventures",
                Price = 599.99m,
                Quantity = 15,
                CategoryId = categories[4].Id // Sports & Outdoors
            }
        };
        
        await context.Products.AddRangeAsync(products);
        
        // No inventory or profit seeding
        await context.SaveChangesAsync();
        
        // Create admin user with all permissions
        var adminUser = new AppUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
            
            // Commented out for now to fix migration issues
            /*
            // Add all permissions to admin
            var adminPermissions = Enum.GetValues(typeof(Permission))
                .Cast<Permission>()
                .Select(p => new UserPermission
                {
                    UserId = adminUser.Id,
                    PermissionName = p.ToString()
                })
                .ToList();
            
            adminUser.Permissions = adminPermissions;
            */
            await userManager.UpdateAsync(adminUser);
        }
        
        // Create category manager user
        var categoryManagerUser = new AppUser
        {
            UserName = "category@example.com",
            Email = "category@example.com",
            FirstName = "Category",
            LastName = "Manager",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };
        
        result = await userManager.CreateAsync(categoryManagerUser, "Category123!");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(categoryManagerUser, "User");
            
            // Commented out for now to fix migration issues
            /*
            // Add CategoryManager permission
            categoryManagerUser.Permissions = new List<UserPermission>
            {
                new UserPermission
                {
                    UserId = categoryManagerUser.Id,
                    PermissionName = Permission.CategoryManager.ToString()
                }
            };
            */
            
            await userManager.UpdateAsync(categoryManagerUser);
        }
        
        // Create product manager user
        var productManagerUser = new AppUser
        {
            UserName = "product@example.com",
            Email = "product@example.com",
            FirstName = "Product",
            LastName = "Manager",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };
        
        result = await userManager.CreateAsync(productManagerUser, "Product123!");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(productManagerUser, "User");
            
            // Commented out for now to fix migration issues
            /*
            // Add ProductManager permission
            productManagerUser.Permissions = new List<UserPermission>
            {
                new UserPermission
                {
                    UserId = productManagerUser.Id,
                    PermissionName = Permission.ProductManager.ToString()
                }
            };
            */
            
            await userManager.UpdateAsync(productManagerUser);
        }
        
        // Create regular user
        var regularUser = new AppUser
        {
            UserName = "user@example.com",
            Email = "user@example.com",
            FirstName = "Regular",
            LastName = "User",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };
        
        result = await userManager.CreateAsync(regularUser, "User123!");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(regularUser, "User");
            // No specific permissions for regular user
        }
        
        await context.SaveChangesAsync();
    }
}