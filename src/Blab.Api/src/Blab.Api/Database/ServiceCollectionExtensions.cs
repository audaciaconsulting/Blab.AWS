using Blab.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Blab.Api.Database;

/// <summary>
/// Service Collection extensions for the database context's.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adding the database context's needed for the API.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<BlabDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(nameof(BlabDbContext))));
}
