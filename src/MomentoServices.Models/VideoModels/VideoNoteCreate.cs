namespace Momento.Services.Models.VideoModels
{
    using Momento.Models.Enums;

    public class VideoNoteCreate
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int? SeekTo { get; set; }

        public Formatting Formatting { get; set; }

        public VideoNoteType Type { get; set; }

        public int InPageId { get; set; }
    
        public int? InPageParentId { get; set; }

        /// <summary>
        ///0 means that it is not root and has parent that is not in the db, -1 means root!
        /// </summary>
        public int? ParentDbId { get; set; }

        /// <summary>
        /// 1 is root!
        /// </summary>
        public int Level { get; set; }

        public bool Deleted { get; set; }
    }
}
