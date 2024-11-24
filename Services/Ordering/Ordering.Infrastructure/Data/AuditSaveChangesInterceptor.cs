using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken); // Correctly returns ValueTask<InterceptionResult<int>>
        }

        private void UpdateAuditFields(DbContext? context)
        {
            if (context == null) return;

            var currentTime = DateTimeOffset.UtcNow;
            var currentUser = "Rooney";

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>().AsEnumerable())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = currentUser;
                    entry.Entity.CreatedAt = currentTime;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedBy = currentUser;
                    entry.Entity.LastModifiedAt = currentTime;
                }
                //else if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable softDeletableEntity)
                //{
                //    // Handle soft delete (if applicable)
                //    softDeletableEntity.DeletedBy = currentUser;
                //    softDeletableEntity.DeletedAt = currentTime;
                //    entry.State = EntityState.Modified; // Prevent physical deletion
                //}
            }
        }
    }
}
