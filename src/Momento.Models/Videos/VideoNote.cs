namespace Momento.Models.Videos
{
    using Momento.Models.Contracts;
    using Momento.Models.Enums;
    using System.Collections.Generic;

    public class VideoNote : SoftDeletableAndTrackable
    {
        public VideoNote()
        {
            this.ChildNotes = new HashSet<VideoNote>();
            this.Formatting = Formatting.Select_Formatting;
            this.Type = VideoNoteType.Note;
        }

        public int Id { get; set; }

        ///Note used, should be removed
        public string  Name { get; set; }

        public string Content { get; set; }

        public int? SeekTo { get; set; }

        public Formatting Formatting { get; set; }

        public VideoNoteType Type { get; set; } 

        public int  Level { get; set; }

        public int Order { get; set; }

        public string  BorderColor { get; set; }

        public string BackgroundColor { get; set; }

        public string TextColor { get; set; }

        public int BorderThickness { get; set; }

        public int? NoteId { get; set; }
        public virtual VideoNote Note { get; set; }

        public int VideoId { get; set; }
        public virtual Video Video { get; set; }

        public virtual ICollection<VideoNote> ChildNotes { get; set; }
    }
}
