namespace Momento.Services.Implementations.ListRemind
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Data.Models.ListsRemind;
    using Momento.Services.Contracts.ListRemind;
    using Momento.Services.Models.List;

    public class ListRemindService : IListRemindService
    {
        private readonly MomentoDbContext context;

        public ListRemindService(MomentoDbContext context)
        {
            this.context = context;
        }

        public List<ListIndex> GetIndex(string userName)
            => context.Lists
               .Where(x => x.User.UserName == userName)
               .Select(x => new ListIndex
               {
                   Id = x.Id,
                   Name = x.Name,
                   ListItemsCount = x.Items.Count,
               })
               .ToList();

        public void Create(string userName, string name, List<ListItemCreate> listItems)
        {
            var userId = context.Users.SingleOrDefault(x => x.UserName == userName).Id;

            var list = CreateList(userId, name, listItems);
            context.Lists.Add(list);
            context.SaveChanges();
        }

        public ListCreate GetEdit(int id)
        {
            var list = context.Lists
                .Include(x => x.Items)
                .SingleOrDefault(x => x.Id == id);

            var listCr = new ListCreate
            {
                Id = list.Id,
                Name = list.Name,
                ListItems = list.Items.Select(x => new ListItemCreate
                {
                    Content = x.Content,
                })
                .ToList(),
            };

            return listCr;
        }
        public void Edit(int listId, string name, List<ListItemCreate> listItems)
        {
            var dbList = context.Lists.SingleOrDefault(x => x.Id == listId);
            var userId = dbList.UserId;
            context.Lists.Remove(dbList);
            var list = CreateList(userId, name, listItems);
            context.Lists.Add(list);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var list = context.Lists.SingleOrDefault(x => x.Id == id);
            context.Lists.Remove(list);
            context.SaveChanges();
        }

        private ListRemind CreateList(string userId, string name, List<ListItemCreate> listItems)
        {
            var list = new ListRemind
            {
                Name = name,
                UserId = userId,
                Items = listItems
                .Select(x => new ListRemindItem
                {
                    Content = x.Content,
                })
                .ToArray()
            };

            return list;
        }

        public string GetName(int id)
        => context.Lists.SingleOrDefault(x => x.Id == id).Name;
    }
}
