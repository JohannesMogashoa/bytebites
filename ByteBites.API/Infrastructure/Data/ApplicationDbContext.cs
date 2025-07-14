using ByteBites.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace ByteBites.API.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure your entity mappings here
        // e.g., modelBuilder.Entity<YourEntity>().ToTable("YourTableName");
    }

    // Define DbSet properties for your entities
    public DbSet<Recipe> Recipes => Set<Recipe>();
}