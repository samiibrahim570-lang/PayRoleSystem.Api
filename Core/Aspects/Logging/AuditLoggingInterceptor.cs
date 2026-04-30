using PayRoleSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace PayRoleSystem.Core.Aspects.Logging
{
    public class AuditLoggingInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLoggingInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var auditLogs = new List<AuditLog>();
            var entries = context.ChangeTracker.Entries<BasicEntity>();
            var loggedInUserId = GetLoggedInUserId();

            try
            {
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                        HandleAddedEntity(entry, loggedInUserId, auditLogs);
                    else if (entry.State == EntityState.Modified)
                        HandleModifiedEntity(entry, loggedInUserId, auditLogs);
                    else if (entry.State == EntityState.Deleted)
                        HandleDeletedEntity(entry, loggedInUserId, auditLogs);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during saving changes: {ex.Message}");
                throw new Exception("Error during saving changes", ex);
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private long GetLoggedInUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value;
            return string.IsNullOrEmpty(userIdClaim) ? 0 : long.Parse(userIdClaim);
        }

        private void HandleAddedEntity(EntityEntry<BasicEntity> entry, long userId, List<AuditLog> auditLogs)
        {
            var entity = entry.Entity;
            entity.GlobalId = Guid.NewGuid();
            entity.CreatedBy = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsDeleted = false;
        }

        private void HandleModifiedEntity(EntityEntry<BasicEntity> entry, long userId, List<AuditLog> auditLogs)
        {
            var entity = entry.Entity;
            entity.ModifiedBy = userId;
            entity.ModifiedOn = DateTime.UtcNow;

            //foreach (var property in entry.Properties.Where(p => p.IsModified && !Equals(p.OriginalValue, p.CurrentValue)))
            //{
            //    auditLogs.Add(new AuditLog
            //    {
            //        EntityName = entity.GetType().Name,
            //        PropertyName = property.Metadata.Name,
            //        OriginalValue = property.OriginalValue?.ToString() ?? string.Empty,
            //        NewValue = property.CurrentValue?.ToString() ?? string.Empty,
            //        ChangedBy = userId,
            //        ChangedOn = DateTime.UtcNow,
            //        Action = "Updated",
            //        Changes = $"Property '{property.Metadata.Name}' updated from '{property.OriginalValue}' to '{property.CurrentValue}'"
            //    });
            //}
        }

        private void HandleDeletedEntity(EntityEntry<BasicEntity> entry, long userId, List<AuditLog> auditLogs)
        {
            var entity = entry.Entity;
            //auditLogs.Add(CreateAuditLog(entity, "Deleted", userId, JsonConvert.SerializeObject(entity)));
        }

        //private AuditLog CreateAuditLog(BasicEntity entity, string action, long userId, string changes)
        //{
        //    return new AuditLog
        //    {
        //        EntityName = entity.GetType().Name,
        //        Action = action,
        //        ChangedBy = userId,
        //        ChangedOn = DateTime.UtcNow,
        //        Changes = changes
        //    };
        //}
         
    }
}
