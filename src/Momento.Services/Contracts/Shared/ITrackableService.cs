namespace Momento.Services.Contracts.Shared
{
    using Momento.Models.Contracts;
    using System;
    using System.Collections.Generic;

    public interface ITrackableService
    {
        void RegisterModification(ITrackable item, DateTime now, bool saveChanges);

        void RegisterViewing(ITrackable item, DateTime now, bool saveChanges);

        void RegisterModificationMany(IEnumerable<ITrackable> items, DateTime now, bool saveChanges);

        void RegisterViewingMany(IEnumerable<ITrackable> items, DateTime now, bool saveChanges);

        void Delete(ISoftDeletable item, DateTime now, bool saveChanges);

        void DeleteMany(IEnumerable<ISoftDeletable> items, DateTime now, bool saveChanges);
    }
}
