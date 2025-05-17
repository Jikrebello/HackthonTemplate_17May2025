using Microsoft.AspNetCore.Authorization;
using MyApp.Common.Constants;

namespace MyApp.API.Authorization;

public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(Permission permission)
    {
        Policy = $"Permission:{permission}";
    }
}
