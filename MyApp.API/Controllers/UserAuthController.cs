using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.Auth;
using MyApp.Common.DTOs.User;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UserAuthController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    // Authentication endpoints
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }

    // User management endpoints
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
            return NotFound();
            
        return Ok(user);
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<UserResponse>> GetUserByUsername(string username)
    {
        var user = await _userService.GetUserByUsernameAsync(username);
        
        if (user == null)
            return NotFound();
            
        return Ok(user);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var user = await _userService.UpdateUserAsync(id, request);
        
        if (user == null)
            return NotFound();
            
        return Ok(user);
    }

    [HttpPut("{id:guid}/password")]
    [Authorize]
    public async Task<ActionResult> UpdatePassword(Guid id, UpdatePasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _userService.UpdatePasswordAsync(id, request);
        
        if (!result)
            return BadRequest("Failed to update password");
            
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
