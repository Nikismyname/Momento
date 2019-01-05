namespace Momento.Services.Models.VideoModels
{
    using System.ComponentModel.DataAnnotations;

    public class VideoInitialCreate
    {
        public int DirectoryId { get; set; }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "The Video Notes length must be between 3 and 40 characters!")]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        public string Description { get; set; }
    }
}
