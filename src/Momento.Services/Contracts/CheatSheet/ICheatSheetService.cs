namespace Momento.Services.Contracts.CheatSheet
{
    using Momento.Models.CheatSheets;
    using Momento.Services.Models.CheatSheets;
    using System.Collections.Generic;

    public interface ICheatSheetService
    {
        CheatSheetCreate[] GetAllCheatSheetsForUser(string userId);

        void CreateCheetSheet(string title, string description, string userId);

        IEnumerable<TopicView> GetAllTopicsWithPoints(int cheatSheetId);

        CheatSheet ById(int id);

        void Delete(int id);
    }
}
