using System.Collections.Generic;
using MyApp.Common.Constants;

namespace MyApp.Common.DTOs.User;

public class UserResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<Permission> Permissions { get; set; } = new List<Permission>();
}
