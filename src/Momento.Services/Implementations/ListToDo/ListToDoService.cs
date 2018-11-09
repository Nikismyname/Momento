namespace Momento.Services.Implementations.ListToDo
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Data.Models.ListsToDo;
    using Momento.Services.Contracts.ListToDo;
    using Momento.Services.Models.ListToDoModels;
    using System;
    using System.Linq;

    public class ListToDoService : IListToDoService
    {
        private readonly MomentoDbContext context;
        private readonly IMapper mapper;

        public ListToDoService(MomentoDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Create(ListToDoCreate modelIn)
        {
            var model = mapper.Map<ListToDo>(modelIn);
            var username = modelIn.UserName;

            var userId = context.Users
                .Select(x => new { id = x.Id, username = x.UserName })
                .SingleOrDefault(x => x.username == username)
                .id;

            model.Id = default(int);
            model.UserId = userId;
            context.ListsTodo.Add(model);
            context.SaveChanges();
        }

        public ListToDoUse GetUseModel(int id)
        {
            var model = context.ListsTodo
                .Include(x=>x.Items)
                .SingleOrDefault(x=>x.Id == id);

            var returnModel = mapper.Map<ListToDoUse>(model);
            return returnModel;
        }

        ///Soft Delete
        public void Delete(int id, string username)
        {
            var listToDo = context.ListsTodo.SingleOrDefault(x => x.Id == id);
            if (listToDo == null)
            {
                throw new Exception("Invalid List To Do");
            }

            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new Exception("Invalid Username!");
            }

            if (listToDo.UserId != user.Id)
            {
                throw new Exception("You can not change someone elses List!");
            }

            var now = DateTime.UtcNow;
            var list = context.ListsTodo.SingleOrDefault(x => x.Id == id);

            if(list == null)
            {
                throw new Exception("List to Delete does not exist!");
            }

            foreach (var item in list.Items)
            {
                item.IsDeleted = true;
                item.DeletedOn = now;
            }

            list.IsDeleted = true;
            list.DeletedOn = now;

            context.SaveChanges();
        }

        public void Save(ListToDoUse model, string username)
        {
            ///Verification 
            var listToDo = context.ListsTodo.SingleOrDefault(x => x.Id == model.Id);
            if(listToDo == null)
            {
                throw new Exception("Invalid List To Do");
            }

            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if(user == null)
            {
                throw new Exception("Invalid Username!");
            }

            if(listToDo.UserId != user.Id)
            {
                throw new Exception("You can not change someone elses List!");
            }

            var validItemIds = context.ListsTodo
                .Where(x => x.Id == model.Id)
                .Select(x => x.Items.Select(y => y.Id))
                .SingleOrDefault()
                .ToArray();

            if (model.Items
                .Where(x=>x.Id > 0)
                .Any(x => !validItemIds.Contains(x.Id)))
            {
                throw new Exception("You can not change someone elses notes, or notes from another List!");
            }

            ///Change Existing Items
            var changedExistingModels = model.Items
                .Where(x => x.Changed == true && x.Id > 0)
                .OrderBy(x=>x.Id)
                .ToArray();

            var changedModelsIds = changedExistingModels
                .Select(x => x.Id).ToArray();

            var dbListItemsChanged = context.ListToDoItems
                .Where(x => changedModelsIds.Contains(x.Id))
                .OrderBy(x=>x.Id)
                .ToArray();

            if(changedExistingModels.Length != dbListItemsChanged.Length)
            {
                throw new Exception("Invalid ListItem ids");
            }

            for (int i = 0; i < changedExistingModels.Length; i++)
            {
                if(changedExistingModels[i].Id != dbListItemsChanged[i].Id)
                {
                    throw new Exception("Invalid ListItem ids");
                }

                dbListItemsChanged[i].Content = changedExistingModels[i].Content;
                dbListItemsChanged[i].Comment = changedExistingModels[i].Comment;
                dbListItemsChanged[i].Status = changedExistingModels[i].Status;
                dbListItemsChanged[i].Order = changedExistingModels[i].Order;
            }

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

            ///Removing Deleted Items
            var allSentIds = model.Items
                .Select(x=>x.Id).ToArray();
            var deletedItems = validItemIds
                .Where(x => !allSentIds.Contains(x));
            var toDeleteDbItems = context.ListToDoItems
                .Where(x => deletedItems.Contains(x.Id))
                .ToArray();
            var now = DateTime.UtcNow;
            for (int i = 0; i < toDeleteDbItems.Length; i++)
            {
                toDeleteDbItems[i].IsDeleted = true;
                toDeleteDbItems[i].DeletedOn = now;
            }

            context.SaveChanges();
        }
    }
}
