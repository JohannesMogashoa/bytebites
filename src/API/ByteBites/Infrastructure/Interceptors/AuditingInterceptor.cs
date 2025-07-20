using ByteBites.Application.Common.Interfaces;
using ByteBites.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ByteBites.Infrastructure.Interceptors;

public class AuditingInterceptor : SaveChangesInterceptor
{
    private readonly IServiceScopeFactory _scopeFactory;

    public AuditingInterceptor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context == null) return;

        // Resolve ICurrentUserService within a new scope to avoid circular dependencies
        // if ICurrentUserService depends on HttpContextAccessor, which itself depends on scoped services.
        // This pattern ensures CurrentUserService is available during SaveChanges.
        using var scope = _scopeFactory.CreateScope();
        var currentUserService = scope.ServiceProvider.GetService<ICurrentUserService>();

        string? currentUserId = currentUserService?.UserId;
        string? currentUserName = currentUserService?.UserName;
        DateTimeOffset now = DateTimeOffset.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                var auditableEntity = entry.Entity;
                auditableEntity.CreatedAt = now;
                auditableEntity.CreatedBy = currentUserName ?? "System"; // Fallback to "System" if no user is authenticated
                auditableEntity.UpdatedAt = now; // Initialize EditedAt as well
                auditableEntity.UpdatedBy = currentUserName;

                // Only set UserId on creation if it's not already set (e.g., for seeded data)
                if (string.IsNullOrEmpty(auditableEntity.UserId))
                {
                    auditableEntity.UserId = currentUserId ?? "System"; // Fallback to "System" if no user is authenticated
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                var auditableEntity = entry.Entity;
                auditableEntity.UpdatedAt = now;
                auditableEntity.UpdatedBy = currentUserName;

                // Important: Mark these properties as Modified *only* if you're not updating the entire entity
                // If you update the whole entity, EF Core will track changes to these properties automatically.
                // If you only set these properties in the interceptor, EF Core won't know they've changed
                // unless you explicitly mark them. This is usually not needed if you update the entity object and then call SaveChanges.
                // However, if for some reason the entity was not fully tracked as modified, this would ensure it.
                // For most cases with RecipeService.UpdateRecipeAsync, the entity is fetched and then modified,
                // so EF Core tracks it automatically. This line might be redundant or needed for specific scenarios.
                // entry.Property(a => a.EditedAt).IsModified = true;
                // entry.Property(a => a.EditedBy).IsModified = true;
            }
        }
    }
}