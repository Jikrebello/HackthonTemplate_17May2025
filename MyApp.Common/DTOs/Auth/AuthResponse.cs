using System.Collections.Generic;
using MyApp.Common.Constants;

namespace MyApp.Common.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<Permission> Permissions { get; set; } = new List<Permission>();
}