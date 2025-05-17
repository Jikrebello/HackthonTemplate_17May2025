namespace MyApp.Domain.Entities;

public class UserPermission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public AppUser User { get; set; } = null!;
}
