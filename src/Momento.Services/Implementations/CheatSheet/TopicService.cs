namespace Momento.Services.Implementations.CheatSheet
{
    using Momento.Data;
    using Momento.Data.Models.CheatSheets;
    using Momento.Services.Contracts.CheatSheet;
    using Momento.Services.Models.CheatSheets;
    using System.Linq;

    public class TopicService : ITopicService
    {
        private readonly MomentoDbContext context;

        public TopicService(MomentoDbContext context)
        {
            this.context = context;
        }

        public void CreateTopic(int sheetId, string topicName)
        {
            var myTopic = new Topic
            {
                CheatSheetId = sheetId,
                Name = topicName,
            };

            context.Topics.Add(myTopic);
            context.SaveChanges();
        }

        public TopicEdit GetTopicEditViewModel(int id)
        => context.Topics
                  .Where(x => x.Id == id)
                  .Select(x => new TopicEdit
                  {
                      SheetId = x.CheatSheetId,
                      Name = x.Name,
                      TopicId = x.Id,
                  })
                  .SingleOrDefault();

        public void Edit(int topicId, string newName)
        {
            var topic = context.Topics.SingleOrDefault(x => x.Id == topicId);
            topic.Name = newName;
            context.SaveChanges();
        }

        public Topic ById(int id)
        => context.Topics.SingleOrDefault(x => x.Id == id);

        public void Delete(int id)
        {
            context.Topics.Remove(context.Topics.SingleOrDefault(x => x.Id == id));
            context.SaveChanges();
        }
    }
}

