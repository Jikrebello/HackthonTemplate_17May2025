using Microsoft.AspNetCore.Identity;

namespace MyApp.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Role { get; set; } = "User";
}