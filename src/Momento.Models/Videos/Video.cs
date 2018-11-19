namespace Momento.Models.Videos
{
    using System.Collections.Generic;
    using Momento.Models.Contracts;
    using Momento.Models.Directories;
    using Momento.Models.Users;

    public class Video : SoftDeletableAndTrackable
    {
        public Video()
        {
            this.Notes = new HashSet<VideoNote>();
        }

        public int Id { get; set; }

        public string  Name { get; set; }

        public string Url { get; set; }

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
