using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> contextOptions) : base(contextOptions) 
        {
            
        }

        public virtual DbSet<Order> Orders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #region Save Changes
        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{
        //    foreach (var entry in ChangeTracker.Entries<BaseEntity>().AsEnumerable())
        //    {
        //        switch (entry.State)
        //        {
        //            case EntityState.Added:
        //                entry.Entity.CreatedBy = "Rooney"; // TODO: Replace with auth server
        //                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
        //                break;
        //            case EntityState.Modified:
        //                entry.Entity.LastModifiedBy = "Rooney"; // TODO: Replace with auth server
        //                entry.Entity.LastModifiedAt = DateTimeOffset.UtcNow;
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //} 
        #endregion
    }
}
