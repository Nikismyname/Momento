namespace Momento.Tests.Utilities
{
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using System.Linq;

    public static class ChangeTrackerOperations
    {
        public static void DetachAll(MomentoDbContext context)
        {
            var changedEntriesCopy = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted ||
                            e.State == EntityState.Unchanged)
                            .ToArray();

            for (int i = 0; i < changedEntriesCopy.Length; i++)
            {
                var entry = changedEntriesCopy[i].Entity;
                context.Entry(entry).State = EntityState.Detached;
            }
        }
    }
}


