namespace Momento.Services.Models.Video
{
    using System.Collections.Generic;

    public class VideoView
    {
        public VideoView()
        {
            this.Notes = new List<VideoNoteView>();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public List<VideoNoteView> Notes { get; set; }
    }
}
