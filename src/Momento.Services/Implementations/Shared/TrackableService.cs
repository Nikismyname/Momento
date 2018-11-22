namespace Momento.Services.Implementations.Shared
{
    using System;
    using System.Collections.Generic;
    using Momento.Data;
    using Momento.Models.Contracts;
    using Momento.Services.Contracts.Shared;

    public class TrackableService : ITrackableService
    {
        private readonly MomentoDbContext context;

        public TrackableService(MomentoDbContext context)
        {
            this.context = context;
        }

        public void RegisterModification(ITrackable item, DateTime now, bool saveChanges)
        {
            item.LastModifiedOn = now;
            item.TimesModified += 1;
            if (saveChanges)
            {
                context.SaveChanges();
            }
        }

        public void RegisterViewing(ITrackable item, DateTime now, bool saveChanges)
        {
            item.LastViewdOn = now;
            item.TimesViewd += 1;
            if (saveChanges)
            {
                context.SaveChanges();
            }
        }

        public void RegisterModificationMany(IEnumerable<ITrackable> items, DateTime now, bool saveChanges)
        {
            foreach (var item in items)
            {
                this.RegisterModification(item, now, false);
            }
            if (saveChanges)
            {
                context.SaveChanges();
            }
        }

        public void RegisterViewingMany(IEnumerable<ITrackable> items, DateTime now, bool saveChanges)
        {
            foreach (var item in items)
            {
                this.RegisterViewing(item, now, false);
            }
            if (saveChanges)
            {
                context.SaveChanges();
            }
        }

        public void Delete(ISoftDeletable item, DateTime now, bool saveChanges)
        {
            item.DeletedOn = now;
            item.IsDeleted = true;
            if (saveChanges)
            {
                context.SaveChanges();
            }
        }

        public void DeleteMany(IEnumerable<ISoftDeletable> items, DateTime now, bool saveChanges)
        {
            foreach (var item in items)
            {
                this.Delete(item, now, false);
            }
            if (saveChanges)
            {
                context.SaveChanges();
            }
        }
    }
}
