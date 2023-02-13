using Blab.DataAccess;
using Blab.DataAccess.OpenIddict;
using Microsoft.EntityFrameworkCore;

namespace Blab.Identity.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseContext(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection.AddDbContext<OpenIddictDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(nameof(OpenIddictDbContext)));

                options.UseOpenIddict<int>();
            })
            .AddDbContext<BlabDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(BlabDbContext))));
    }
}
