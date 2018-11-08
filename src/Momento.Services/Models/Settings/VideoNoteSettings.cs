namespace Momento.Services.Models.Settings
{
    public class VideoNoteSettings
    {
        public bool VNPauseVideoOnTopNewNote { get; set; }
        public bool VNPauseVideoOnBottomNewNote { get; set; }
        public bool VNPauseVideoOnSubNoteTop { get; set; }
        public bool VNPauseVideoOnSubNoteRegular { get; set; }
        public bool VNPauseVideoOnTopicTop { get; set; }
        public bool VNPauseVideoOnTopicBottom { get; set; }
        public bool VNPauseVideoOnTimeStampTop { get; set; }
        public bool VNPauseVideoOnTimeStampBottom { get; set; }

        public bool VNGoDownOnNewNoteTop { get; set; }
        public bool VNGoDownOnSubNoteAll { get; set; }
        public bool VNGoDownOnNewTopicTop { get; set; }
        public bool VNGoDownOnNewTimeStampTop { get; set; }

        public bool VNAutoSaveProgress { get; set; }
    }
}
