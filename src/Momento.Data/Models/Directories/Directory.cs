namespace Momento.Data.Models.Directories
{
    using System.ComponentModel.DataAnnotations;
    using Momento.Data.Models.ListsToDo;
    using Momento.Data.Models.Users;
    using System.Collections.Generic;
    using Momento.Data.Models.Videos;
    using Momento.Data.Models.Contracts;
    using System;

    public class Directory : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public Directory()
        {
            this.Videos = new HashSet<Video>();
            this.Subdirectories = new HashSet<Directory>();
            this.IsDeleted = false;
        }

        public string  Name { get; set; }

        public int  Order { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int?  ParentDirectoryId { get; set; }
        public virtual Directory ParentDirectory { get; set; }


        public virtual ICollection<Video> Videos { get; set; }

        public virtual ICollection<ListToDo> ListsToDo { get; set; }

        public virtual ICollection<Directory> Subdirectories { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
