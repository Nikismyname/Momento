﻿namespace Momento.Models.ListsToDo
{
    using Momento.Models.Contracts;
    using Momento.Models.Directories;
    using Momento.Models.Users;
    using System;
    using System.Collections.Generic;

    public class ListToDo : SoftDeletableAndTrackable, IOrderable<int>
    {
        public ListToDo()
        {
            this.Items = new HashSet<ListToDoItem>();
            this.Categories = "highPriority;active;backBurner;doneNeedsFixes;done;unassigned";
        }

        public int  Id { get; set; }

        public int Order { get; set; }

        public string  UserId { get; set; }
        public virtual User User { get; set; }

        public int  DirectoryId { get; set; }
        public virtual Directory Directory { get; set; }

        public string  Name { get; set; }

        public string  Description { get; set; }

        public string Categories { get; set; }

        public virtual ICollection<ListToDoItem> Items { get; set; }
    }
}
