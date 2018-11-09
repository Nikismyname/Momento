namespace Momento.Services.Implementations.ListRemind
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Data.Models.ListsRemind;
    using Momento.Services.Contracts.ListRemind;
    using Momento.Services.Models.ListRemind;

    public class ListRemindService : IListRemindService
    {
        private readonly MomentoDbContext context;

        public ListRemindService(MomentoDbContext context)
        {
            this.context = context;
        }

        public List<ListRemindIndex> GetIndex(string userName)
            => context.ListsRemind
               .Where(x => x.User.UserName == userName)
               .Select(x => new ListRemindIndex
               {
                   Id = x.Id,
                   Name = x.Name,
                   ListItemsCount = x.Items.Count,
               })
               .ToList();

        public void Create(string userName, string name, List<ListRemindItemCreate> listItems)
        {
            var userId = context.Users.SingleOrDefault(x => x.UserName == userName).Id;

            var list = CreateList(userId, name, listItems);
            context.ListsRemind.Add(list);
            context.SaveChanges();
        }

        public ListRemindCreate GetEdit(int id)
        {
            var list = context.ListsRemind
                .Include(x => x.Items)
                .SingleOrDefault(x => x.Id == id);

            var listCr = new ListRemindCreate
            {
                Id = list.Id,
                Name = list.Name,
                ListItems = list.Items.Select(x => new ListRemindItemCreate
                {
                    Content = x.Content,
                })
                .ToList(),
            };

            return listCr;
        }
        public void Edit(int listId, string name, List<ListRemindItemCreate> listItems)
        {
            var dbList = context.ListsRemind.SingleOrDefault(x => x.Id == listId);
            var userId = dbList.UserId;
            context.ListsRemind.Remove(dbList);
            var list = CreateList(userId, name, listItems);
            context.ListsRemind.Add(list);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var list = context.ListsRemind.SingleOrDefault(x => x.Id == id);
            context.ListsRemind.Remove(list);
            context.SaveChanges();
        }

        private ListRemind CreateList(string userId, string name, List<ListRemindItemCreate> listItems)
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
        => context.ListsRemind.SingleOrDefault(x => x.Id == id).Name;
    }
}
