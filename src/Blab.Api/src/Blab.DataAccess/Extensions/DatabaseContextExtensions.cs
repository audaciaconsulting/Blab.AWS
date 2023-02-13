using Blab.DataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Blab.DataAccess.Extensions;
public static class DatabaseContextExtensions
{
    public static void Configure<TEntity>(this ModelBuilder modelBuilder, IEntityTypeConfiguration<TEntity> configuration) where TEntity : class
    {
        configuration.Configure(modelBuilder.Entity<TEntity>());
    }
}
