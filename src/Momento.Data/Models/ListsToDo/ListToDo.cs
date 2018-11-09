namespace Momento.Data.Models.ListsToDo
{
    using Momento.Data.Models.Contracts;
    using Momento.Data.Models.Directories;
    using Momento.Data.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ListToDo : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public ListToDo()
        {
            this.Items = new HashSet<ListToDoItem>();
            this.Categories = "highPriority;active;backBurner;doneNeedsFixes;done;unassigned";
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
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
