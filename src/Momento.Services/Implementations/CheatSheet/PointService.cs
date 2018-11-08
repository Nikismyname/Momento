namespace Momento.Services.Implementations.CheatSheet
{
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Data.Models.CheatSheets;
    using Momento.Data.Models.Enums;
    using Momento.Services.Contracts.CheatSheet;
    using Momento.Services.Models.CheatSheets;
    using System.Linq;

    public class PointService : IPointService
    {
        private readonly MomentoDbContext context;

        public PointService(MomentoDbContext context)
        {
            this.context = context;
        }

        public void Create(int topicId, string name, string content, Formatting? formatting)
        {
            var point = new Point
            {
                TopicId = topicId,
                Name = name,
                Content = content,
                Formatting = (Formatting)formatting,
            };

            context.Points.Add(point);
            context.SaveChanges();
        }

        public PointCreate GetEditModel(int id)
        {
            var point = context.Points
                .Include(x => x.Topic)
                .SingleOrDefault(x => x.Id == id);

            var result = new PointCreate
            {
                Content = point.Content,
                Formatting = point.Formatting,
                Name = point.Name,
                SheetId = point.Topic.CheatSheetId,
                TopicId = point.Topic.Id,
            };

            return result;
        }

        public void Delete(int pointId)
        {
            context.Points.Remove(context.Points.SingleOrDefault(x=>x.Id == pointId));
            context.SaveChanges();
        }

        public void Edit(int pointId, string name, string content/*, string contentFormatted, string preview, string previewFormatted*/, Formatting? formatting)
        {
            var point = context.Points.SingleOrDefault(x => x.Id == pointId);
            point.Name = name;
            point.Content = content;
            //point.ContentFormatted = contentFormatted;
            //point.Preview = preview;
            //point.PreviewFormatted = previewFormatted;
            point.Formatting = (Formatting)formatting;

            context.SaveChanges();
        }
    }
}
