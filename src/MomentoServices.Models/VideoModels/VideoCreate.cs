namespace Momento.Services.Models.VideoModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VideoCreate
    {
        public VideoCreate()
        {
            Notes = new List<VideoNoteCreate>();
        }

        public int Id { get; set; }

        public int DirectoryId { get; set; }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "The Video Notes length must be between 3 and 40 characters!")]
        public string Name { get; set; }

        public string Url { get; set; }

        public int? SeekTo { get; set; }

        public string Description { get; set; }

        public int  Order { get; set; }

        public List<VideoNoteCreate> Notes { get; set; }
    }
}
