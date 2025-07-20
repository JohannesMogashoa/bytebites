using ByteBites.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ByteBites.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure your entity mappings here
        // e.g., modelBuilder.Entity<YourEntity>().ToTable("YourTableName");
    }

    // Define DbSet properties for your entities
    public DbSet<Recipe> Recipes => Set<Recipe>();
}