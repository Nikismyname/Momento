namespace Momento.Web.Models.Video
{
    using Momento.Services.Models.Video;
    using System.Collections.Generic;

    public class VideoCreatePartial
    {
        public VideoCreatePartial()
        {
            Notes = new List<VideoNoteCreate>();
        }

        public VideoNoteCreate Note { get; set; }

        public List<VideoNoteCreate> Notes { get; set; }
    }
}
