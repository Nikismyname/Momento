namespace Momento.Services.Implementations.Directory
{
    using Data;
    using Contracts.Directory;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public class ReorderingService : IReorderingService
    {
        private readonly MomentoDbContext context;

        public ReorderingService(MomentoDbContext context)
        {
            this.context = context;

        }

        public void SaveItemsForOneDir(int parentDir, string cntOrDir, int[] orderings)
        {
            if (cntOrDir == "dir")
            {
                var directories = context.Directories
                    .Include(x => x.Subdirectories)
                    .SingleOrDefault(x => x.Id == parentDir)
                    .Subdirectories
                    .ToArray();

                for (int i = 0; i < orderings.Length; i++)
                {
                    var directoryId = orderings[i];
                    var dir = directories.SingleOrDefault(x => x.Id == directoryId);

                    if (dir.Order != i)
                    {
                        dir.Order = i;
                    }
                }
            }
            else if(cntOrDir == "cnt")
            {
                var contents = context.Directories
                   .Include(x => x.Videos)
                   .SingleOrDefault(x => x.Id == parentDir)
                   .Videos
                   .ToArray();

                for (int i = 0; i < orderings.Length; i++)
                {
                    var contentId = orderings[i];
                    var cnt = contents.SingleOrDefault(x => x.Id == contentId);

                    if (cnt.Order != i)
                    {
                        cnt.Order = i;
                    }
                }
            }

            context.SaveChanges();
        }
    }
}

























//Dictionary<string, Dictionary<int, Dictionary<int, int>>> dirs = new Dictionary<string, Dictionary<int, Dictionary<int, int>>>();
//Dictionary<string, Dictionary<int, Dictionary<int, int>>> cnts = new Dictionary<string, Dictionary<int, Dictionary<int, int>>>();

//public void AddOrdering(string cntOrDir, string user, int parentDirId, int[] ordering)
//{
//    var dict = cntOrDir == "cnt" ? cnts : dirs;

//    if (dict.ContainsKey(user) == false)
//    {
//        dict[user] = new Dictionary<int, Dictionary<int, int>>();
//    }

//    dict[user][parentDirId] = new Dictionary<int, int>();

//    for (var i = 0; i < ordering.Length; i++)
//    {
//        var dirId = ordering[i, 0];
//        var dirPos = ordering[i, 1];
//        dict[user][parentDirId].Add(dirId, dirPos);
//    }
//}

//public void SaveOrderingForUser(string user)
//{
//    var change1 = false;
//    var change2 = false;

//    if (dirs.ContainsKey(user) && dirs[user].Any())
//    {
//        change1 = SaveDirs(dirs[user]);
//        dirs[user].Clear();
//    }

//    if (cnts.ContainsKey(user) && cnts[user].Any())
//    {
//        change2 = SaveCnts(dirs[user]);
//        cnts[user].Clear();
//    }

//    if (change1 || change2)
//    {
//        context.SaveChanges();
//    }
//}

//private bool SaveCnts(Dictionary<int, Dictionary<int, int>> data)
//{
//    bool diff = false;
//    foreach (var kvp in data)
//    {
//        var contents = context.Directories
//        .Include(x => x.Contents)
//        .SingleOrDefault(x => x.Id == kvp.Key)
//        .Contents
//        .ToArray();

//        foreach (var orderKvp in kvp.Value)
//        {
//            var cont = contents.SingleOrDefault(x => x.Id == orderKvp.Key);
//            if (cont.Order != orderKvp.Value)
//            {
//                diff = true;
//                cont.Order = orderKvp.Value;
//            }
//        }
//    }

//    return diff;
//}

//private bool SaveDirs(Dictionary<int, Dictionary<int, int>> data)
//{
//    bool diff = false;
//    foreach (var kvp in data)
//    {
//        var directories = context.Directories
//        .Include(x => x.Subdirectories)
//        .SingleOrDefault(x => x.Id == kvp.Key)
//        .Subdirectories
//        .ToArray();

//        foreach (var orderKvp in kvp.Value)
//        {
//            var dir = directories.SingleOrDefault(x => x.Id == orderKvp.Key);
//            if (dir.Order != orderKvp.Value)
//            {
//                diff = true;
//                dir.Order = orderKvp.Value;
//            }
//        }
//    }
//    return diff;
//}