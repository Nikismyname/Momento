namespace Momento.Services.Models.VideoModels
{
    using Momento.Services.Models.Settings;

    public class VideoCreateWithSettings
    {
        public VideoCreate ContentCreate { get; set; } 

        public VideoNoteSettings Settings { get; set; }

        public string Mode { get; set; }
    }
}
