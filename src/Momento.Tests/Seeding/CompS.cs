namespace Momento.Tests.Seeding
{
    using Momento.Data;
    using Momento.Models.Comparisons;
    using System.Collections.Generic;
    using System.Linq;

    public static class CompS
    {
        public static Comparison[] SeedTwoCompsToUser(MomentoDbContext context, string userId)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);

            var comps = new List<Comparison>();

            comps.Add(new Comparison
            {
                Description = "",
                Name = "",
                Order = 0,
                TargetLanguage = "",
                SourceLanguage = "",
                UserId = user.Id,
                DirectoryId = user.Directories.SingleOrDefault().Id,
            });

            comps.Add(new Comparison
            {
                Description = "",
                Name = "",
                Order = 0,
                TargetLanguage = "",
                SourceLanguage = "",
                UserId = user.Id,
                DirectoryId = user.Directories.SingleOrDefault().Id,
            });

            context.AddRange(comps);
            context.SaveChanges();

            return comps.ToArray();
        }

        public static ComparisonItem[] SeedItemsToComp(MomentoDbContext context, Comparison comp)
        {
            var items = new HashSet<ComparisonItem>
            {
                new ComparisonItem
                {
                    Id = 1,
                    Comment = "item1Comment",
                    Order =1,
                    Target ="item1Target",
                    Source ="item1Source",
                },
                new ComparisonItem
                {
                    Id = 2,
                    Comment = "item2Comment",
                    Order =2,
                    Target ="item2Target",
                    Source ="item2Source",
                },
                new ComparisonItem
                {
                    Id = 3,
                    Comment = "item3Comment",
                    Order =3,
                    Target ="item3Target",
                    Source ="item3Source",
                },
            };

            comp.Items = items;
            context.SaveChanges();

            return items.ToArray();
        }
    }
}
