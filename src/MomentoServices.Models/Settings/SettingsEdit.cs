namespace Momento.Services.Models.Settings
{
    using Momento.Models.Enums;

    public class SettingsEdit
    {
        public string Username { get; set; }

        public int Id { get; set; }

        public CSSTheme CSSTheme { get; set; }

        public bool DarkInputs { get; set; }


        public bool PauseVideoOnTopNewNote { get; set; }

        public bool PauseVideoOnBottomNewNote { get; set; }

        public bool PauseVideoOnTopSubNoteTop { get; set; }

        public bool PauseVideoOnTopicTop { get; set; }

        public bool PauseVideoOnTopicBottom { get; set; }

        public bool PauseVideoOnTimeStampTop { get; set; }

        public bool PauseVideoOnTimeStampBottom { get; set; }

        public bool GoDownOnNewNoteTop { get; set; }

        public bool GoDownOnNewTopicTop { get; set; }

        public bool GoDownOnNewTimeStampTop { get; set; }
    }
}
