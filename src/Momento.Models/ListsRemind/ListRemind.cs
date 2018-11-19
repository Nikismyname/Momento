namespace Momento.Models.ListsRemind
{
    using Momento.Models.Contracts;
    using Momento.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ListRemind : SoftDeletableAndTrackable
    {
        public ListRemind()
        {
            this.Items = new HashSet<ListRemindItem>();
        }

        public int  Id  { get; set; }

        public string  Name { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<ListRemindItem> Items { get; set; }
    }
}
