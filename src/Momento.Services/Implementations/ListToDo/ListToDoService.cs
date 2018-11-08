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
        public void Delete(int id)
        {
            var now = DateTime.UtcNow;
            var list = context.ListsTodo.SingleOrDefault(x => x.Id == id);

            foreach (var item in list.Items)
            {
                item.IsDeleted = true;
                item.DeletedOn = now;
            }

            list.IsDeleted = true;
            list.DeletedOn = now;

            context.SaveChanges();
        }

        /// <summary>
        /// Through deleting and replacing for now;
        /// </summary>
        public void Save(ListToDoUse model, string username)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == username);

            if(user == null)
            {
                throw new Exception("User with that username not found!");
            }

            var oldList = context.ListsTodo
                .SingleOrDefault(x => x.Id == model.Id);
            if (oldList == null)
            {
                throw new Exception("List to modify not found!");
            }
            context.ListsTodo.Remove(oldList);

            if(oldList.UserId != user.Id)
            {
                throw new Exception("You can not modify someone elses list!");
            }

            var newList = new ListToDo
            {
                Categories = model.Categories,
                Description = model.Description,
                DirectoryId = model.DirectoryId,
                Name = model.Name,
                UserId = user.Id,
                Items = model.Items.Select(x=>new ListToDoItem
                {
                     Comment = x.Comment,
                     Content = x.Content,
                     Status = x.Status,
                     Order = x.Order,
                })
                .ToArray(),
            };

            context.ListsTodo.Add(newList);
            context.SaveChanges();
        }
    }
}
