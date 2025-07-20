using System.Linq.Expressions;
using ByteBites.Domain;
using ByteBites.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ByteBites.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var softDeleteEntities = typeof(ISoftDelete).Assembly.GetTypes()
            .Where(type => typeof(ISoftDelete)
                               .IsAssignableFrom(type)
                           && type.IsClass
                           && !type.IsAbstract);
        
        foreach (var softDeleteEntity in softDeleteEntities)
        {
            builder.Entity(softDeleteEntity).HasQueryFilter(
                GenerateQueryFilterLambda(softDeleteEntity));
        }
    }

    private LambdaExpression? GenerateQueryFilterLambda(Type type)
    {
        var parameter = Expression.Parameter(type, "w");
        var falseConstantValue = Expression.Constant(false);
        var propertyAccess = Expression.PropertyOrField(parameter, nameof(ISoftDelete.IsDeleted));
        var equalExpression = Expression.Equal(propertyAccess, falseConstantValue);
        var lambda = Expression.Lambda(equalExpression, parameter);

        return lambda;
    }

    // Define DbSet properties for your entities
    public DbSet<Recipe> Recipes => Set<Recipe>();
}