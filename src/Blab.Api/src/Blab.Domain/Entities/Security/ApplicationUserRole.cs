using Microsoft.AspNetCore.Identity;

namespace Blab.Domain.Entities.Security;

public class ApplicationUserRole : IdentityUserRole<int>
{
    public override int UserId { get; set; }

    public override int RoleId { get; set; }

    public virtual ApplicationUser? User { get; set; }

    public virtual ApplicationRole? Role { get; set; }
}
