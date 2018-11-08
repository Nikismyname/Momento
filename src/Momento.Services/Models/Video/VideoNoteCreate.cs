namespace Momento.Services.Models.Video
{
    using Momento.Data.Models.Enums;

    public class VideoNoteCreate
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int? SeekTo { get; set; }

        public Formatting Formatting { get; set; }

        public VideoNoteType Type { get; set; }

        public int InPageId { get; set; }

        public int? InPageParentId { get; set; }

        public int? ParentDbId { get; set; }

        public int Level { get; set; }

        public bool Deleted { get; set; }
    }
}
