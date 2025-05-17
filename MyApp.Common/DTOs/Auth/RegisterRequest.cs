using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyApp.Common.Constants;

namespace MyApp.Common.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public List<Permission> Permissions { get; set; } = new List<Permission>();
}