namespace Momento.Data.Models.ListsRemind
{
    using Momento.Data.Models.Contracts;
    using Momento.Data.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ListRemind : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public ListRemind()
        {
            this.Items = new HashSet<ListRemindItem>();
        }

        public string  Name { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<ListRemindItem> Items { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
