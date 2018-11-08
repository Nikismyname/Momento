namespace Momento.Services.Models.Video
{
    using System.Collections.Generic;

    public class VideoCreate
    {
        public VideoCreate()
        {
            Notes = new List<VideoNoteCreate>();
        }

        public int Id { get; set; }

        public int DirectoryId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int? SeekTo { get; set; }

        public string Description { get; set; }

        public int  Order { get; set; }

        public List<VideoNoteCreate> Notes { get; set; }
    }
}
