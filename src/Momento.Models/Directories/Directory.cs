﻿namespace Momento.Models.Directories
{
    using System.ComponentModel.DataAnnotations;
    using Momento.Models.ListsToDo;
    using Momento.Models.Users;
    using System.Collections.Generic;
    using Momento.Models.Videos;
    using Momento.Models.Contracts;
    using System;

    public class Directory : SoftDeletableAndTrackable
    {
        public Directory()
        {
            this.Videos = new HashSet<Video>();
            this.Subdirectories = new HashSet<Directory>();
        }

        public int Id { get; set; }

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
    }
}
