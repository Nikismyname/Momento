namespace Momento.Models.Videos
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Momento.Models.Contracts;
    using Momento.Models.Directories;
    using Momento.Models.Users;

    public class Video : SoftDeletableAndTrackable, IOrderable<int>
    {
        public Video()
        {
            this.Notes = new HashSet<VideoNote>();
        }

        public int Id { get; set; }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "The Video Notes length must be between 3 and 40 characters!")]
        public string Name { get; set; }

        ///must be valid but I am doing this check front end
        [Required]
        public string Url { get; set; }

        ///No requirements
        public string Description { get; set; }

        public int Order { get; set; }

        public int? SeekTo { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int  DirectoryId { get; set; }
        public virtual Directory Directiry { get; set; }

        public virtual ICollection<VideoNote> Notes { get; set; }
    }
}
