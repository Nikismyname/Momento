namespace Momento.Services.Implementations.ListToDo
{
    #region Initialization 
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Models.ListsToDo;
    using Momento.Services.Contracts.Directory;
    using Momento.Services.Contracts.ListToDo;
    using Momento.Services.Contracts.Other;
    using Momento.Services.Contracts.Shared;
    using Momento.Services.Exceptions;
    using Momento.Services.Models.ListToDoModels;
    using System;
    using System.Linq;

    public class ListToDoService : IListToDoService
    {
        private readonly MomentoDbContext context;
        private readonly ITrackableService trackableService;
        private readonly IUserService userService;
        private readonly IReorderingService reorderingService;

        public ListToDoService(
            MomentoDbContext context,
            ITrackableService trackableService,
            IUserService userService,
            IReorderingService reorderingService)
        {
            this.context = context;
            this.trackableService = trackableService;
            this.userService = userService;
            this.reorderingService =reorderingService;
        }
        #endregion

        //Authenticated
        //Registers user for the list
        public void Create(ListToDoCreate pageListToDo, string username)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var direcotory = context.Directories.SingleOrDefault(x => x.Id == pageListToDo.DirectoryId);
            if(direcotory == null)
            {
                throw new ItemNotFound("The directory for this List does not exists");
            }

            if(direcotory.UserId != user.Id)
            {
                throw new AccessDenied("The directory you are trying to create you List does not belong to you!");
            }

            var order = 0;
            var ids = direcotory.ListsToDo.Select(x => x.Order).ToArray();
            if(ids.Length != 0) {
                order = ids.Max() + 1;
            }

            var newListToDo = Mapper.Instance.Map<ListToDo>(pageListToDo);
            newListToDo.Order = order;
            newListToDo.Id = default(int);
            newListToDo.UserId = user.Id;
            context.ListsTodo.Add(newListToDo);
            context.SaveChanges();
        }

        //Authorizes 
        //Registers View 
        public ListToDoUse GetUseModel(int id, string username)
        {
            var model = context.ListsTodo
                .Include(x=>x.Items)
                .SingleOrDefault(x=>x.Id == id);

            var userId = userService.GetUserId(username);
            if(userId != model.UserId)
            {
                throw new AccessDenied("The list you are trying to access dos not belong to you!");
            }

            trackableService.RegisterViewing(model, DateTime.Now, true);

            var returnModel = Mapper.Instance.Map<ListToDoUse>(model);
            return returnModel;
        }

        //Soft Deletes
        //authorizes
        public void Delete(int id, string username)
        {
            var listToDo = context.ListsTodo.SingleOrDefault(x => x.Id == id);
            if (listToDo == null)
            {
                ///trowing that so people can not find valid ids through sending delete requests 
                throw new ItemNotFound("The list you are trying to delete does not exist!");
            }

            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            if (listToDo.UserId != user.Id)
            {
                throw new AccessDenied("The list you are trying to delete does not belong to you!");
            }

            var now = DateTime.UtcNow;

            ///Using lazy loading?
            trackableService.DeleteMany(listToDo.Items, now, false);
            trackableService.Delete(listToDo, now, false);

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

        #region SAVE
        //Verified 
        //Regesters Deletion
        //Registers Modifications
        public void Save(ListToDoUse model, string username)
        {
            ListToDo listToDo;
            int[] validItemIds;

            this.VerifyListToSaveBelongsToUser(model, username, out listToDo);

            this.ChangeExistingItems(model, out validItemIds);

            this.trackableService.RegisterModification(listToDo, DateTime.UtcNow, false);

            this.PersistNewItems(model, listToDo);

            this.RemoveDeletedItems(model, validItemIds);

            context.SaveChanges();
        }

        private void VerifyListToSaveBelongsToUser(ListToDoUse model, string username, out ListToDo listToDo)
        {
            listToDo = context.ListsTodo.SingleOrDefault(x => x.Id == model.Id);
            if (listToDo == null)
            {
                throw new ItemNotFound("The list you are trying to modify does not exist!");
            }

            var userId = context.Users.SingleOrDefault(x => x.UserName == username)?.Id;
            if (userId == null)
            {
                throw new UserNotFound(username);
            }

            if (listToDo.UserId != userId)
            {
                throw new AccessDenied("The list you are trying to modify does not beling to you!");
            }
        }

        private void ChangeExistingItems(ListToDoUse model, out int[] validItemIds)
        {
            this.VerifyThatToBeChangedIdsBelongToCurrentList(model,  out validItemIds);
           
            var changedPageModels = model.Items
                .Where(x => x.Changed == true && x.Id > 0)
                .OrderBy(x => x.Id)
                .ToArray();

            var changedModelsIds = changedPageModels
                .Select(x => x.Id).ToArray();

            var toBeChangedDbModels = context.ListToDoItems
                .Where(x => changedModelsIds.Contains(x.Id))
                .OrderBy(x => x.Id)
                .ToArray();

            if (changedPageModels.Length != toBeChangedDbModels.Length)
            {
                throw new InternalServerError("Error occurred trying to apply changes to modified listToDo items");
            }

            for (int i = 0; i < changedPageModels.Length; i++)
            {
                if (changedPageModels[i].Id != toBeChangedDbModels[i].Id)
                {
                    throw new InternalServerError("Error occurred trying to apply changes to modified listToDo items");
                }

                toBeChangedDbModels[i].Content = changedPageModels[i].Content;
                toBeChangedDbModels[i].Comment = changedPageModels[i].Comment;
                toBeChangedDbModels[i].Status = changedPageModels[i].Status;
                toBeChangedDbModels[i].Order = changedPageModels[i].Order;
            }

            this.trackableService.RegisterModificationMany(toBeChangedDbModels, DateTime.UtcNow, false);
        }

        private void VerifyThatToBeChangedIdsBelongToCurrentList(ListToDoUse model, out int[] validItemsIds)
        {
            ///Getting the ids if the items that exest in the database 
            validItemsIds = context.ListsTodo
                .SingleOrDefault(x => x.Id == model.Id)
                .Items
                .Select(x => x.Id)
                .ToArray();

            ///If the user sends id for modification that is not 
            ///in existring items return an exception
            foreach (var item in model.Items.Where(x=>x.Id > 0))
            {
                if (!validItemsIds.Contains(item.Id))
                {
                    throw new AccessDenied("The items you are trying to modify do not belong to the current list!");
                }
            }
        }

        private void PersistNewItems(ListToDoUse model, ListToDo listToDo)
        {
            ///Adding new items
            var itemsToAdd = model.Items
                .Where(x => x.Id == 0)
                .ToArray();

            var dbNewItems = itemsToAdd.Select(x => new ListToDoItem
            {
                ListToDoId = listToDo.Id,
                Comment = x.Comment,
                Content = x.Content,
                Status = x.Status,
                Order = x.Order,
            })
                .ToArray();

            context.ListToDoItems.AddRange(dbNewItems);
        }

        private void RemoveDeletedItems(ListToDoUse model, int[] validItemIds)
        {
            ///Removing Deleted Items
            var allSentIds = model.Items
                .Select(x => x.Id).ToArray();
            var deletedItems = validItemIds
                .Where(x => !allSentIds.Contains(x));
            var toDeleteDbItems = context.ListToDoItems
                .Where(x => deletedItems.Contains(x.Id))
                .ToArray();

            trackableService.DeleteMany(toDeleteDbItems, DateTime.UtcNow, false);
        }
        #endregion
    }
}
