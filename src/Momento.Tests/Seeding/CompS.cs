namespace Momento.Tests.Seeding
{
    using Momento.Data;
    using Momento.Models.Comparisons;
    using System.Collections.Generic;
    using System.Linq; 

    public static class CompS
    {
        public const int CompItem1Id = 1;
        public const int CompItem1Order = 0;
        public const string CompItem1Description = "ItemOneDescription";
        public const string CompItem1Name = "ItemOneName";
        public const string CompItem1Target = "ItemOneTarget";
        public const string CompItem1Source = "ItemOneSource";

        public const int CompItem2Id = 2;
        public const int CompItem2Order = 1;
        public const string CompItem2Description = "ItemOneDescription";
        public const string CompItem2Name = "ItemOneName";
        public const string CompItem2Target = "ItemOneTarget";
        public const string CompItem2Source = "ItemOneSource";

        public static Comparison[] SeedTwoCompsToUser(MomentoDbContext context, string userId, int? dirId = null )
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);

            var directory = dirId == null ?
                user.Directories.Single(x => x.ParentDirectoryId == null) :
                context.Directories.Single(x => x.Id == dirId);

            var comps = new List<Comparison>();

            comps.Add(new Comparison
            {
                Id = dirId==null? CompItem1Id : 0,
                Order = CompItem1Order,
                Description = CompItem1Description,
                Name = CompItem1Name,
                TargetLanguage = CompItem1Target,
                SourceLanguage = CompItem1Source,
                UserId = user.Id,
                DirectoryId = directory.Id,
            });

            comps.Add(new Comparison
            {
                Id = dirId == null ? CompItem2Id : 0,
                Order = CompItem2Order,
                Description = CompItem2Description,
                Name = CompItem2Name,
                TargetLanguage = CompItem2Target,
                SourceLanguage = CompItem2Source,
                UserId = user.Id,
                DirectoryId = directory.Id,
            });

            context.AddRange(comps);
            context.SaveChanges();

            return comps.ToArray();
        }

        public const int Item1Id = 1;
        public const int Item1Order = 0;
        public const string Item1Comment = "item1Comment";
        public const string Item1Target = "item1Target";
        public const string Item1Source = "item1Source";

        public const int Item2Id = 2;
        public const int Item2Order = 1;
        public const string Item2Comment = "item2Comment";
        public const string Item2Target = "item2Target";
        public const string Item2Source = "item2Source";

        public const int Item3Id = 3;
        public const int Item3Order = 2;
        public const string Item3Comment = "item3Comment";
        public const string Item3Target = "item3Target";
        public const string Item3Source = "item3Source";

        public static ComparisonItem[] SeedThreeItemsToComp(MomentoDbContext context, Comparison comp, bool autoGenerateIds = false)
        {
            var items = new HashSet<ComparisonItem>
            {
                new ComparisonItem
                {
                    Id = autoGenerateIds? 0 : Item1Id,
                    Order =Item1Order,
                    Comment = Item1Comment,
                    Target = Item1Target,
                    Source = Item1Source,
                },
                new ComparisonItem
                {
                    Id = autoGenerateIds? 0 : Item2Id,
                    Order =Item2Order,
                    Comment = Item2Comment,
                    Target = Item2Target,
                    Source = Item2Source,
                },
                new ComparisonItem
                {
                    Id = autoGenerateIds? 0 : Item3Id,
                    Order =Item3Order,
                    Comment = Item3Comment,
                    Target = Item3Target,
                    Source = Item3Source,
                },
            };

            comp.Items = items;
            context.SaveChanges();

            return items.ToArray();
        }
    }
}
