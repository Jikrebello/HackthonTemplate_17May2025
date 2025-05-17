using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyApp.Common.Constants;

namespace MyApp.Common.DTOs.User;

public class UpdateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty;
    
    public List<Permission> Permissions { get; set; } = new List<Permission>();
}
