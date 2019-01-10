namespace Momento.Services.Implementations.Directory
{
    using Data;
    using Contracts.Directory;
    using System.Linq;
    using Momento.Models.Contracts;
    using System.Collections.Generic;
    using Exceptions;
    using Microsoft.EntityFrameworkCore;

    public class ReorderingService : IReorderingService
    {
        public const string ComparisonType = "comparison";
        public const string ListToDoType = "listToDo";
        public const string NoteType = "note";
        public const string VideoType = "video";
        public const string DirectoryType = "directory"; 

        private readonly MomentoDbContext context;

        public ReorderingService(MomentoDbContext context)
        {
            this.context = context;
        }

        ///Tested: comp
        public void Reorder(string type, int dir, int[][] ItemIdNewOrderKVP, string username)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var query = this.context.Directories
                .Where(x => x.Id == dir);

            if (query.Count() != 1)
            {
                throw new ItemNotFound("the directorie in witch you want to reorder items does not exist!");
            }

            if (query.Select(x => x.UserId).Single() != user.Id)
            {
                throw new AccessDenied("The directory you are trying to reorder things in does not belong to you!");
            }

            var dirWithItems = new Momento.Models.Directories.Directory();

            var itemsToBeRoorderd = new List<IOrderable<int>>();
            switch (type)
            {
                case ComparisonType:
                    dirWithItems = query.Include(x => x.Comparisons).Single();
                    itemsToBeRoorderd = dirWithItems.Comparisons.Select(x => x as IOrderable<int>).ToList();
                    break;
                case ListToDoType:
                    dirWithItems = query.Include(x => x.ListsToDo).Single();
                    itemsToBeRoorderd = dirWithItems.ListsToDo.Select(x => x as IOrderable<int>).ToList();
                    break;
                case NoteType:
                    dirWithItems = query.Include(x => x.Notes).Single();
                    itemsToBeRoorderd = dirWithItems.Notes.Select(x => x as IOrderable<int>).ToList();
                    break;
                case VideoType:
                    dirWithItems = query.Include(x => x.Videos).Single();
                    itemsToBeRoorderd = dirWithItems.Videos.Select(x => x as IOrderable<int>).ToList();
                    break;
                case DirectoryType:
                    dirWithItems = query.Include(x => x.Subdirectories).Single();
                    itemsToBeRoorderd = dirWithItems.Subdirectories.Select(x => x as IOrderable<int>).ToList();
                    break;
            }

            var idsFromDb = itemsToBeRoorderd.Select(x => x.Id).OrderBy(x => x).ToArray();
            var idsFromReorderRequest = ItemIdNewOrderKVP.Select(x => x[0]).OrderBy(x => x).ToArray();

            ///Making sure all the data sent is correct and coresponds to dbData
            if (idsFromDb.Length != idsFromReorderRequest.Length) throw new BadRequestError("The send ids do not match the length of the ids found in the database!");
            if (idsFromReorderRequest.Distinct().ToArray().Length != idsFromReorderRequest.Length) throw new BadRequestError("There were repeating ids in the reorder requent!");
            for (int i = 0; i < idsFromDb.Length; i++)
            {
                if (idsFromDb[i] != idsFromReorderRequest[i])
                {
                    throw new BadRequestError("the ids send in the reorder request do not match the ids in the database!");
                };
            };
            ///...

            for (int i = 0; i < idsFromDb.Length; i++)
            {
                var id = idsFromDb[i];
                var itemToBeReordered = itemsToBeRoorderd.Single(x => x.Id == id);
                var newValue = ItemIdNewOrderKVP.Single(x => x[0] == id)[1];

                itemToBeReordered.Order = newValue;
            }

            context.SaveChanges();
        }
    }
}