namespace Momento.Tests.Seeding
{
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Models.ListsToDo;
    using System.Collections.Generic;
    using System.Linq;

    public static class ListTDS
    {
        public const string categories = "cat1;cat2;cat3;Unassigned;cat4";
        public const string desctiption = "magnificent description";
        public const string name = "magnificent name";
        public const int order = 0;

        public static ListToDo SeedListToUser(MomentoDbContext context, string username)
        {
            var user = context.Users
                .Include(x => x.Directories)
                .SingleOrDefault(x => x.UserName == username);

            var list = new ListToDo
            {
                Categories = categories,
                Description = desctiption,
                DirectoryId = user.Directories.Single().Id,
                Name = name,
                UserId = user.Id,
                Items = new HashSet<ListToDoItem>(),
                Order = order,
            };

            context.ListsTodo.Add(list);
            context.SaveChanges();
            return list;
        }

        public const string item1Comment = "itemOneComment";
        public const string item1Content = "itemOneContent";
        public const string item1Status = "itemOneStatus";
        public const int item1Order = 0;

        public const string item2Comment = "itemTwoComment";
        public const string item2Content = "itemTwoContent";
        public const string item2Status = "itemTwoStatus";
        public const int item2Order = 1;

        public static ListToDoItem[] SeedTwoItemsToList(MomentoDbContext context, ListToDo list)
        {
            var items = new List<ListToDoItem>
            {
                new ListToDoItem
                {
                     Comment =item1Comment,
                     Content =item1Content,
                     Order = item1Order,
                     Status =item1Status,
                },

                new ListToDoItem
                {
                     Comment =item2Comment,
                     Content =item2Content,
                     Order = item2Order,
                     Status = item2Status,
                },
            };

            foreach (var item in items)
            {
                list.Items.Add(item);
            }
            context.SaveChanges();

            return items.ToArray();
        }
    }
}
