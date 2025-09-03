using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFramework;

public sealed class EfDatabaseContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public EfDatabaseContext(DbContextOptions<EfDatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfDatabaseContext).Assembly);
    }
}
