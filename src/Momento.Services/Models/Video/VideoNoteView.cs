namespace Momento.Services.Models.Video
{
    using System.Collections.Generic;
    using System.Xml;

    public class VideoNoteView
    {
        public VideoNoteView()
        {
            ChildNotes = new List<VideoNoteView>();
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public string Preview { get; set; }

        public int? SeekTo { get; set; }

        public int Level { get; set; }

        public Formatting Formatting { get; set; }

        public ICollection<VideoNoteView> ChildNotes { get; set; }
    }
}
