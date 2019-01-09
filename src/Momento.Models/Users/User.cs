namespace Momento.Models.Users
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using Momento.Models.ListsRemind;
    using Momento.Models.ListsToDo;
    using Momento.Models.Videos;
    using Momento.Models.Directories;
    using Momento.Models.Contracts;
    using Momento.Models.Notes;

    public class User : IdentityUser, ISoftDeletableAndTrackable
    {
        public User()
        {
            this.Directories = new HashSet<Directory>();
            this.ListsToDo = new HashSet<ListToDo>();
            this.ListsRemind = new HashSet<ListRemind>();
            this.Videos = new HashSet<Video>();
            this.Comparisons = new HashSet<Comparisons.Comparison>();
            this.Notes = new HashSet<Note>();

            this.IsDeleted = false;
            var now = DateTime.UtcNow;
            this.CreatedOn = now;
            this.LastModifiedOn = now;
            this.LastViewdOn = now;
            this.DeletedOn = null;
            this.TimesModified = 1;
            this.TimesViewd = 1;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserSettings UserSettings { get; set; }

        //public virtual Directory Root { get; set; }

        public ICollection<Directory> Directories { get; set; }

        public ICollection<Video> Videos { get; set; }

        public ICollection<ListRemind> ListsRemind { get; set; }

        public ICollection<ListToDo> ListsToDo { get; set; }

        public ICollection<Comparisons.Comparison> Comparisons { get; set; }

        public ICollection<Note> Notes { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? LastViewdOn { get; set; }
        public int TimesModified { get; set; }
        public int TimesViewd { get; set; }
    }
}
