using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.API.Authorization;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [RequirePermission("users.view.all")]
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
            
        // Only allow users to access their own data unless they have the permission
        if (User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value != id.ToString() && 
            !User.HasClaim(c => c.Type == "permission" && c.Value == "users.view.any"))
        {
            return Forbid();
        }
            
        return Ok(user);
    }

    [HttpGet("username/{username}")]
    [RequirePermission("users.view.any")]
    public async Task<ActionResult<UserResponse>> GetUserByUsername(string username)
    {
        var user = await _userService.GetUserByUsernameAsync(username);
        
        if (user == null)
            return NotFound();
            
        return Ok(user);
    }

    [HttpPost]
    [RequirePermission("users.create")]
    public async Task<ActionResult<UserResponse>> CreateUser(CreateUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var user = await _userService.CreateUserAsync(request);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        // Only allow users to update their own data unless they have the permission
        if (User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value != id.ToString() && 
            !User.HasClaim(c => c.Type == "permission" && c.Value == "users.update.any"))
        {
            return Forbid();
        }
        
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
            
        // Only allow users to update their own password unless they have the permission
        if (User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value != id.ToString() && 
            !User.HasClaim(c => c.Type == "permission" && c.Value == "users.update.any"))
        {
            return Forbid();
        }
        
        var result = await _userService.UpdatePasswordAsync(id, request);
        
        if (!result)
            return BadRequest("Failed to update password");
            
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("users.delete")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }

    [HttpPost("{id:guid}/permissions")]
    [RequirePermission("users.permissions.manage")]
    public async Task<ActionResult> AddPermission(Guid id, [FromBody] string permission)
    {
        if (string.IsNullOrEmpty(permission))
            return BadRequest("Permission cannot be empty");
            
        var result = await _userService.AddPermissionAsync(id, permission);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }

    [HttpDelete("{id:guid}/permissions/{permission}")]
    [RequirePermission("users.permissions.manage")]
    public async Task<ActionResult> RemovePermission(Guid id, string permission)
    {
        if (string.IsNullOrEmpty(permission))
            return BadRequest("Permission cannot be empty");
            
        var result = await _userService.RemovePermissionAsync(id, permission);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
