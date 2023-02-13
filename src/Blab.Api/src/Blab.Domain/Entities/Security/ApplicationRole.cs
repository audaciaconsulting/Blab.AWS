using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Blab.Domain.Entities.Security;

public class ApplicationRole : IdentityRole<int>
{
    public DateTime? Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? Modified { get; set; }

    public string? ModifiedBy { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<ApplicationUserRole> Roles { get; set; } = new List<ApplicationUserRole>();
}
