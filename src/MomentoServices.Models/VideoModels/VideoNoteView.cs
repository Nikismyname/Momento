namespace Momento.Services.Models.VideoModels
{
    using Momento.Models.Enums;
    using Momento.Models.Videos;
    using Momento.Services.Models.Contracts;
    using System.Collections.Generic;

    public class VideoNoteView : IMapFrom<VideoNote>
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
