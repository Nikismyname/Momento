namespace Momento.Data.Models.Videos
{
    using System;
    using System.Collections.Generic;
    using Momento.Data.Models.Contracts;
    using Momento.Data.Models.Directories;
    using Momento.Data.Models.Users;

    public class Video : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public Video()
        {
            this.Notes = new HashSet<VideoNote>();
            this.IsDeleted = false;
        }

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

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
