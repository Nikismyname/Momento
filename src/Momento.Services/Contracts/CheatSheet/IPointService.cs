namespace Momento.Services.Contracts.CheatSheet
{
    using Momento.Models.Enums;
    using Momento.Services.Models.CheatSheets;

    public interface IPointService
    {
        void Create(int topicId,string name, string content, Formatting? formatting);

        PointCreate GetEditModel(int id);

        void Delete(int pointId);

        void Edit(int topicId, string name, string content, /*string contentFormatted, string preview, string previewFormatted,*/ Formatting? formatting);
    }
}
