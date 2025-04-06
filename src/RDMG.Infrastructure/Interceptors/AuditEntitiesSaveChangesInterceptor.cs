using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure.Interceptors;
public class AuditEntitiesSaveChangesInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AuditEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        AuditEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AuditEntities(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Modified:
                default:
                    break;
            }

            if (entry.State is not (EntityState.Added or EntityState.Modified))
                continue;
            entry.Entity.LastModifiedBy = _currentUserService.UserId;
            entry.Entity.LastModified = DateTime.UtcNow;
        }
    }
}