namespace Momento.Data.Models.Users
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using Momento.Data.Models.CheatSheets;
    using Momento.Data.Models.Codes;
    using Momento.Data.Models.ListsRemind;
    using Momento.Data.Models.ListsToDo;
    using Momento.Data.Models.Videos;
    using Momento.Data.Models.Directories;
    using Momento.Data.Models.Contracts;

    public class User : IdentityUser, IBaseModel<string>, IChangeAndSoftDeleteTrackable
    {
        public User()
        {
            this.Directories = new HashSet<Directory>();
            this.ListsToDo = new HashSet<ListToDo>();
            this.ListsRemind = new HashSet<ListRemind>();
            this.ListsToDo = new HashSet<ListToDo>();
            this.CodeSnippets = new HashSet<Code>();
            this.CheatSheets = new HashSet<CheatSheet>();
            this.Videos = new HashSet<Video>();
            this.IsDeleted = false;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual UserSettings UserSettings { get; set; }

        public virtual ICollection<Directory> Directories { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        public virtual ICollection<CheatSheet> CheatSheets { get; set; }

        public virtual ICollection<Code> CodeSnippets { get; set; }

        public virtual ICollection<ListRemind> ListsRemind { get; set; }

        public virtual ICollection<ListToDo> ListsToDo { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
