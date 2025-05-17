using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;

namespace MyApp.Application.Interfaces.Persistence;

public interface IAppDbContext
{
    DbSet<Product> Products { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
