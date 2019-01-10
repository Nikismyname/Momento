﻿namespace Momento.Services.Implementations.Comparisons
{
    #region Initialization
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Momento.Models.Comparisons;
    using Momento.Models.Users;
    using Momento.Services.Contracts.Comparisons;
    using Exceptions;
    using Models.Attributes;
    using Models.ComparisonModels;
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
        ///Tested
        public ComparisonEdit GetForEdit(int compId, string username, bool isAdmin = false)
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

            if (exstComp.UserId != user.Id && isAdmin == false)
            {
                throw new AccessDenied("This comparison does not belong to you!");
            }

            var result = Mapper.Instance.Map<ComparisonEdit>(exstComp);
            return result;
        }

        public ComparisonEdit GetForEditApi(int compId, string username, bool isAdmin = false)
        {
            try
            {
                var result = GetForEdit(compId, username, isAdmin);
                return result;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Create
        ///Tested
        public Comparison Create(ComparisonCreate data, string username, bool isAdmin = false)
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
            if(parentDir.UserId != user.Id && isAdmin == false)
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

            return comparison;
        } 

        public bool CreateApi(ComparisonCreate data, string username, bool isAdmin = false)
        {
            try
            {
                this.Create(data, username, isAdmin);
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
        ///Tested
        public void Save(ComparisonSave saveData, string username, bool isAdmin = false)
        {
            User user = null;
            Comparison comp = null;
            this.ValidateSave(saveData, username, ref user, ref comp, isAdmin);
            this.SetComparisonFields(saveData, comp);
            this.AlterExistingItems(saveData, comp);
            this.SaveNewItems(saveData, comp);
            context.SaveChanges();
        }

        public bool SaveApi(ComparisonSave saveData, string username, bool isAdmin = false)
        {
            try
            {
                this.Save(saveData, username, isAdmin);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ValidateSave(ComparisonSave saveData, string username,ref User user, ref Comparison comp, bool isAdmin)
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

            if (comp.UserId != user.Id && isAdmin == false)
            {
                throw new AccessDenied("The comparison does not belong to you!");
            }
        }

        ///Sets the new value if the recieved argument is not null
        private void SetComparisonFields(ComparisonSave saveData, Comparison comp)
        {
            var comparisonProperties = saveData
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            comparisonProperties = comparisonProperties
                .Where(x => x.GetCustomAttributes<ComparisonPropertyAttribute>().Count() > 0)
                .ToArray();

            comparisonProperties = comparisonProperties
                .Where(x => x.GetValue(saveData) != null)
                .ToArray();

            foreach (var compProp in comparisonProperties)
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
        #endregion

        #region Delete
        ///Tested
        public Comparison Delete(int id, string username, bool isAdmin = false)
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

            if(comp.UserId != user.Id && isAdmin == false)
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

            return comp;
        }

        public bool DeleteApi(int id, string username, bool isAdmin = false)
        {
            try
            {
                this.Delete(id, username, isAdmin);
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
