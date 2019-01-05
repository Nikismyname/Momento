namespace Momento.Services.Models.VideoModels
{
    using System.ComponentModel.DataAnnotations;

    public class VideoSave
    {
        public int VideoId { get; set; }

        public int? SeekTo { get; set; }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "The Video Notes length must be between 3 and 40 characters!")]

        public string Name { get; set; }

        public string Description { get; set; }

        public string[][] Changes { get; set; }

        public VideoNoteCreate[] NewNotes { get; set; }

        public bool FinalSave { get; set; }
    }
}
