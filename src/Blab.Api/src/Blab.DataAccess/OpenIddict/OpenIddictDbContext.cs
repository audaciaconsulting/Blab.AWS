using Microsoft.EntityFrameworkCore;

namespace Blab.DataAccess.OpenIddict;

public class OpenIddictDbContext : DbContext
{
    public OpenIddictDbContext(DbContextOptions<OpenIddictDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.UseOpenIddict<int>();
        base.OnModelCreating(builder);
    }
}
