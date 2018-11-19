﻿namespace Momento.Models.ListsToDo
{
    using Momento.Models.Contracts;
    using Momento.Models.Directories;
    using Momento.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ListToDo : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public ListToDo()
        {
            this.Items = new HashSet<ListToDoItem>();
            this.Categories = "highPriority;active;backBurner;doneNeedsFixes;done;unassigned";
            this.CreatedOn = DateTime.UtcNow;
        }

        public string  UserId { get; set; }
        public virtual User User { get; set; }

        public int  DirectoryId { get; set; }
        public virtual Directory Directory { get; set; }

        public string  Name { get; set; }

        public string  Description { get; set; }

        public string Categories { get; set; }

        public virtual ICollection<ListToDoItem> Items { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
