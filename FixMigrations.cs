using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Infrastructure.Data;

namespace MyApp.MigrationFix
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create a new service collection
            var services = new ServiceCollection();
            
            // Add the DbContext to the service collection
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql("Host=localhost;Database=MyApp;Username=postgres;Password=password"));
            
            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();
            
            // Get the DbContext from the service provider
            using var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            
            // Manually insert the missing migration records
            await dbContext.Database.ExecuteSqlRawAsync(@"
                INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                SELECT '20250517101724_AddUserPermissions', '9.0.5'
                WHERE NOT EXISTS (SELECT 1 FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = '20250517101724_AddUserPermissions');

                INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                SELECT '20250517110313_AddedPendingChanges', '9.0.5'
                WHERE NOT EXISTS (SELECT 1 FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = '20250517110313_AddedPendingChanges');

                INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                SELECT '20250517110725_AddBarcodeToProduct', '9.0.5'
                WHERE NOT EXISTS (SELECT 1 FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = '20250517110725_AddBarcodeToProduct');
            ");
            
            Console.WriteLine("Migration history has been updated successfully.");
        }
    }
}
