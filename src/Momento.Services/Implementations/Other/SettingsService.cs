namespace Momento.Services.Implementations.Other
{
    using Momento.Data;
    using Momento.Data.Models.Users;
    using Momento.Services.Contracts.Other;
    using System.Linq;
    using Momento.Data.Attributes;
    using Momento.Services.Models.Settings;

    public class SettingsService : ISettingsService
    {
        private readonly MomentoDbContext context;
        private readonly IUserService userService;

        public SettingsService(MomentoDbContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public UserSettings GetSettings(string email)
        {
            var settings = context.Users
                .Select(x => new { email = x.Email, userSettings = x.UserSettings })
                .SingleOrDefault(x => x.email == email)
                .userSettings;

            if (settings == null)
            {
                settings = CreateSettings(email);
            }

            return settings;
        }

        public UserSettings CreateSettings(string email)
        {
            var settings = new UserSettings();
            var user = userService.ByEmail(email);
            user.UserSettings = settings;
            context.SaveChanges();
            return settings;
        }

        public void Edit(UserSettings newSetting)
        {
            var setting = context.UsersSettings.SingleOrDefault(x => x.Id == newSetting.Id);

            var props = setting.GetType().GetProperties();
            props = props.Where(x=>x.GetCustomAttributes(typeof(SettingAttribute),false).Count() > 0).ToArray();

            foreach (var prop in props)
            {
                var dbVal = prop.GetValue(setting);
                var inputVal = prop.GetValue(newSetting);

                if (dbVal != inputVal)
                {
                    prop.SetValue(setting, inputVal);
                }
            }

            context.SaveChanges();
        }

        public VideoNoteSettings GetVideoNoteSettings(string username)
        {
            var user = context.Users
                .Select(x => new
                {
                    un = x.UserName,
                    settings = new VideoNoteSettings
                    {
                        VNGoDownOnNewNoteTop = x.UserSettings.VNGoDownOnNewNoteTop,
                        VNGoDownOnNewTimeStampTop = x.UserSettings.VNGoDownOnNewTimeStampTop,
                        VNGoDownOnNewTopicTop = x.UserSettings.VNGoDownOnNewTopicTop,
                        VNPauseVideoOnBottomNewNote = x.UserSettings.VNPauseVideoOnBottomNewNote,
                        VNPauseVideoOnSubNoteTop = x.UserSettings.VNPauseVideoOnSubNoteTop,
                        VNPauseVideoOnTimeStampBottom = x.UserSettings.VNPauseVideoOnTimeStampBottom,
                        VNPauseVideoOnTimeStampTop = x.UserSettings.VNPauseVideoOnTimeStampTop,
                        VNPauseVideoOnTopicBottom = x.UserSettings.VNPauseVideoOnTopicBottom,
                        VNPauseVideoOnTopicTop = x.UserSettings.VNPauseVideoOnTopicTop,
                        VNPauseVideoOnTopNewNote = x.UserSettings.VNPauseVideoOnTopNewNote,
                        VNGoDownOnSubNoteAll = x.UserSettings.VNGoDownOnSubNoteAll,
                        VNPauseVideoOnSubNoteRegular = x.UserSettings.VNPauseVideoOnSubNoteRegular,
                        VNAutoSaveProgress = x.UserSettings.VNAutoSaveProgress,
                    }
                })
                .SingleOrDefault(x => x.un == username);

            return user.settings;
        }
    }
}
