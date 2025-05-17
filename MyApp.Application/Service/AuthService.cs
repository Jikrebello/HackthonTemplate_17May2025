using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyApp.Common.Constants;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.Auth;
using MyApp.Domain.Entities;

namespace MyApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _config;

    public AuthService(UserManager<AppUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow,
            // Commented out for now to fix migration issues
            // Permissions = new List<UserPermission>(),
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        // Add permissions if any are specified
        foreach (var permission in request.Permissions)
        {
            user.Permissions.Add(new UserPermission
            {
                UserId = user.Id,
                PermissionName = permission.ToString()
            });
        }

        if (user.Permissions.Any())
        {
            await _userManager.UpdateAsync(user);
        }

        return await GenerateJwt(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user =
            await _userManager.FindByNameAsync(request.UserNameOrEmail)
            ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        return await GenerateJwt(user);
    }

    private async Task<AuthResponse> GenerateJwt(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
        };
        
        // Commented out for now to fix migration issues
        /*
        // Add permissions as claims and collect for response
        var permissions = new List<Permission>();
        foreach (var permission in user.Permissions)
        {
            // Try to parse the permission name to our enum
            if (Enum.TryParse<Permission>(permission.PermissionName, out var permissionEnum))
            {
                claims.Add(new Claim("permission", permissionEnum.ToString()));
                permissions.Add(permissionEnum);
            }
        }
        */
        var permissions = new List<Permission>();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"]!)),
            signingCredentials: creds
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            UserName = user.UserName!,
            Permissions = permissions
        };
    }
}