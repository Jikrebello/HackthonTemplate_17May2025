using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MyApp.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Role { get; set; } = "User";
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    // Commented out for now to fix migration issues
    // public virtual ICollection<UserPermission> Permissions { get; set; } = new List<UserPermission>();
}