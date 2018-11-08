namespace Momento.Services.Contracts.Other
{
    using Momento.Data.Models.Users;
    using Momento.Services.Models.Settings;

    public interface ISettingsService
    {
        UserSettings GetSettings(string email);

        UserSettings CreateSettings(string email);

        void Edit(UserSettings newSetting);

        VideoNoteSettings GetVideoNoteSettings(string username);
    }
}
