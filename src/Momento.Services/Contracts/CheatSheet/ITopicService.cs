namespace Momento.Services.Contracts.CheatSheet
{
    using Momento.Models.CheatSheets;
    using Momento.Services.Models.CheatSheets;

    public interface ITopicService
    {
        void CreateTopic(int sheetId, string topic);

        TopicEdit GetTopicEditViewModel(int id);

        void Edit(int topicId, string newName);

        Topic ById(int id);

        void Delete(int id);
    }
}
