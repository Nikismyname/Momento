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

        public int Order { get; set; }

        public string Content { get; set; }

        public int? SeekTo { get; set; }

        public int Level { get; set; }

        ///Border Items
        public string BorderColor { get; set; }

        public string BackgroundColor { get; set; }

        public string TextColor { get; set; }

        public int BorderThickness { get; set; }
        ///...

        public VideoNoteType Type { get; set; }

        public Formatting Formatting { get; set; }

        public ICollection<VideoNoteView> ChildNotes { get; set; }
    }
}
