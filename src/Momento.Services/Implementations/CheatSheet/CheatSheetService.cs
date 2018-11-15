namespace Momento.Services.Implementations.CheatSheet
{
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Models.CheatSheets;
    using Momento.Services.Contracts.CheatSheet;
    using Momento.Services.Models.CheatSheets;
    using System.Collections.Generic;
    using System.Linq;

    public class CheatSheetService : ICheatSheetService
    {
        private MomentoDbContext context;

        public CheatSheetService(MomentoDbContext context)
        {
            this.context = context;
        }

        public CheatSheetCreate[] GetAllCheatSheetsForUser(string userName)
        {
            var sheets = context.CheatSheets
                .Where(x => x.User.UserName == userName)
                .Select(x => new CheatSheetCreate
                {
                    Id = x.Id,
                    Title = x.Name,
                    Description = x.Description,
                })
                .ToArray();

            return sheets;
        }

        public void CreateCheetSheet(string title, string description, string userId)
        {
            var sheet = new CheatSheet
            {
                Description = description,
                Name = title,
                UserId = userId
            };

            context.CheatSheets.Add(sheet);
            context.SaveChanges();
        }

        public IEnumerable<TopicView> GetAllTopicsWithPoints(int cheatSheetId)
        {
            var result = context.CheatSheets
                .Include(x => x.Topics)
                    .ThenInclude(x => x.Points)
                .SingleOrDefault(x => x.Id == cheatSheetId)
                .Topics.Select(x => new TopicView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Points = x.Points.Select(y => new PointPreview
                    {
                         Id = y.Id,
                         Preview = y.Content,
                    })
                     .ToArray()
                })
                .ToList();
            return result;
        }

        public CheatSheet ById(int id)
        => context.CheatSheets.SingleOrDefault(x => x.Id == id);

        public void Delete(int id)
        {
            var sheet = context.CheatSheets.SingleOrDefault(x=>x.Id==id);
            context.CheatSheets.Remove(sheet);
            context.SaveChanges();
        }
    }
}
