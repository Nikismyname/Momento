namespace Momento.Services.Implementations.SoftDelete
{
    using Momento.Data;
    using Momento.Models.Contracts;
    using Momento.Models.Videos;
    using Momento.Services.Contracts.SoftDelete;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SoftDeleteService : ISoftDeleteService
    {
        private readonly MomentoDbContext context;

        public SoftDeleteService(MomentoDbContext context)
        {
            this.context = context;
        }

        public void SoftCascadeDelete(ISoftDeletable model)
        {
            var deleteTime = DateTime.UtcNow;
            this.SoftCascadeDelete(model, deleteTime);
            context.SaveChanges();
        }

        private void SoftCascadeDelete(ISoftDeletable model, DateTime deleteTime)
        {
            var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var wantedProperties = properties
                .Where(x =>
                    x.PropertyType.IsGenericType &&
                    x.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>) &&
                    typeof(ISoftDeletable).IsAssignableFrom(x.PropertyType.GetGenericArguments()[0]))
                .ToArray();

            foreach (var prop in wantedProperties)
            {
                ///can not cast even though models are implementing the interface - the problem 
                ///might be with differenet intances of the library some assemmbly stuff
                var listOfChildItems = (HashSet<ISoftDeletable>)prop.GetValue(model);

                if (typeof(VideoNote).IsAssignableFrom(prop.PropertyType.GetGenericArguments()[0]))
                {
                    foreach (var item in listOfChildItems)
                    {
                        item.DeletedOn = deleteTime;
                        item.IsDeleted = true;
                    }

                    continue;
                }

                foreach (var childItem in listOfChildItems)
                {
                    SoftCascadeDelete(childItem, deleteTime);
                }
            }

            model.DeletedOn = deleteTime;
            model.IsDeleted = true;
        }
    }
}
