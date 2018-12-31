namespace Momento.Services.Implementations.Comparisons
{
    #region Initialization
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Models.Comparisons;
    using Momento.Models.Users;
    using Momento.Services.Contracts.Comparisons;
    using Momento.Services.Exceptions;
    using Momento.Services.Models.Attributes;
    using Momento.Services.Models.ComparisonModels;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public class ComparisonService : IComparisonService
    {
        private readonly MomentoDbContext context;

        public ComparisonService(MomentoDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetForEdit
        public ComparisonEdit GetForEdit(int compId, string username)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var exstComp = context.Comparisons
                .Include(x => x.Items)
                .SingleOrDefault(x => x.Id == compId);

            if (exstComp == null)
            {
                throw new ItemNotFound("The comparison you are looking for does not exist!");
            }

            if (exstComp.UserId != user.Id)
            {
                throw new AccessDenied("This comparison does not belong to you!");
            }

            var result = Mapper.Instance.Map<ComparisonEdit>(exstComp);
            return result;
        }

        public ComparisonEdit GetForEditApi(int compId, string username)
        {
            try
            {
                var result = GetForEdit(compId, username);
                return result;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Create

        public void Create(ComparisonCreate data, string username)
        {
            var parentDirId = data.ParentDirId;

            var user = this.context.Users.SingleOrDefault(x => x.UserName == username);
            if(user == null)
            {
                throw new UserNotFound(username);
            }

            var parentDir = this.context.Directories.SingleOrDefault(x => x.Id == parentDirId);

            if (parentDir == null)
            {
                throw new ItemNotFound("The parent directory for the comparison you are trying to create does not exist!");
            }

            ///Check the Dir belongs to the user
            if(parentDir.UserId != user.Id)
            {
                throw new AccessDenied("The directory you are trying to put the comparison in does not belong to you!");
            }

            var numberOfExistingComparisonsInGivenDirectory = this.context.Directories.
                Where(x => x.Id == parentDirId)
                .Select(x => x.Comparisons.Count)
                .SingleOrDefault();
            var order = numberOfExistingComparisonsInGivenDirectory;

            var comparison = new Comparison
            {
                UserId = user.Id,
                DirectoryId = parentDir.Id,
                Order = order,
                Name = data.Name,
                Description = data.Description,
                TargetLanguage = data.TargetLanguage,
                SourceLanguage= data.SourceLanguage,    
            };

            context.Comparisons.Add(comparison);
            context.SaveChanges();
        } 

        public bool CreateApi(ComparisonCreate data, string username)
        {
            try
            {
                this.Create(data, username);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Save
        ///validated
        public void Save(ComparisonSave saveData, string username)
        {
            User user = null;
            Comparison comp = null;
            this.ValidateSave(saveData, username, ref user, ref comp);
            this.SetCompatisonFields(saveData, comp);
            this.AlterExistingItems(saveData, comp);
            this.SaveNewItems(saveData, comp);
            context.SaveChanges();
        }

        private void ValidateSave(ComparisonSave saveData, string username,ref User user, ref Comparison comp)
        {
            user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            comp = context.Comparisons.SingleOrDefault(x => x.Id == saveData.Id);
            if (comp == null)
            {
                throw new ItemNotFound("The comparison you are trying to modify does not exist!");
            }

            if (comp.UserId != user.Id)
            {
                throw new AccessDenied("The comparison does not belong to you!");
            }
        }

        ///Sets the new value if the recieved argument is not null
        private void SetCompatisonFields(ComparisonSave saveData, Comparison comp)
        {
            var comparisonPropertis = saveData
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            comparisonPropertis = comparisonPropertis
                .Where(x => x.GetCustomAttributes<ComparisonPropertyAttribute>().Count() > 0)
                .ToArray();

            comparisonPropertis = comparisonPropertis
                .Where(x => x.GetValue(saveData) != null)
                .ToArray();

            foreach (var compProp in comparisonPropertis)
            {
                var matchingPropInDbModel = comp
                    .GetType()
                    .GetProperty(compProp.Name, BindingFlags.Public | BindingFlags.Instance);
                var newValue = compProp.GetValue(saveData);
                matchingPropInDbModel.SetValue(comp, newValue);
            }
        }

        private void AlterExistingItems(ComparisonSave saveData, Comparison comp)
        {
            var validItemIdsForAltering = context.ComparisonItems
                .Where(x => x.ComparisonId == comp.Id)
                .Select(x => x.Id)
                .ToArray();

            var idsToAlterDistict = saveData.AlteredItems.Select(x => x.Id).Distinct().ToArray();

            var dbItemsToAlter = context.ComparisonItems
                .Where(x => x.ComparisonId == comp.Id && idsToAlterDistict.Contains(x.Id))
                .ToArray();

            if (idsToAlterDistict.Any(x=> !validItemIdsForAltering.Contains(x)))
            {
                throw new AccessDenied("The comparison items you are trying to alter does not belong the comparison you are altering!");
            }

            ///DELETION
            var idsToDelete = saveData.AlteredItems.Where(x=>x.PropertyChanged=="deleted")
                .Select(x=>x.Id)
                .ToArray();

            if (idsToDelete.Length != idsToDelete.Distinct().ToArray().Length)
            {
                throw new Exception("deletion should be send once per item");
            }

            var now = DateTime.UtcNow;
            foreach (var id in idsToDelete)
            {
                var currentItem = dbItemsToAlter.Single(x => x.Id == id);
                currentItem.DeletedOn = now;
                currentItem.IsDeleted = true;
            }

            ///Removing the deleted items from other changes that might have happened to them;
            saveData.AlteredItems = saveData.AlteredItems.Where(x=> !idsToDelete.Contains(x.Id)).ToHashSet();

            foreach (var alteredItem in saveData.AlteredItems)
            {
                var propToAlter = typeof(ComparisonItem)
                    .GetProperty(alteredItem.PropertyChanged, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                var propType = propToAlter.PropertyType;

                var typeConverter = TypeDescriptor.GetConverter(propType);
                var newValue = typeConverter.ConvertFromString(alteredItem.NewValue);

                var dbItemToAlter = dbItemsToAlter.SingleOrDefault(x => x.Id == alteredItem.Id);

                propToAlter.SetValue(dbItemToAlter, newValue);
            }
        }

        private void SaveNewItems(ComparisonSave saveData, Comparison comp)
        {
            var newItems = saveData.NewItems.Select(x => Mapper.Map<ComparisonItem>(x)).ToArray();
            ///TODO: Maybe some validation here sql injection or script attack;
            foreach (var newItem in newItems)
            {
                newItem.ComparisonId = comp.Id;
            }

            this.context.ComparisonItems.AddRange(newItems);
        }

        public bool SaveApi(ComparisonSave saveData, string username)
        {
            try
            {
                this.Save(saveData, username);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        #endregion

        #region Delete
        public void Delete(int id, string username)
        {
            var user = this.context.Users.SingleOrDefault(x => x.UserName == username);
            if(user == null)
            {
                throw new UserNotFound(username);
            }

            var comp = this.context.Comparisons
                .Include(x=>x.Items)
                .SingleOrDefault(x => x.Id == id);
            if(comp == null)
            {
                throw new ItemNotFound("The comparison you are trying to delete does not exist!");
            }

            if(comp.UserId != user.Id)
            {
                throw new AccessDenied("The comparison you are trying to delete does not belong to you!");
            }

            var now = DateTime.UtcNow;

            foreach (var item in comp.Items)
            {
                item.DeletedOn = now;
                item.IsDeleted = true;
            }

            comp.DeletedOn = now;
            comp.IsDeleted = true;

            context.SaveChanges();
        }

        public bool DeleteApi(int id, string username)
        {
            try
            {
                this.Delete(id, username);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
