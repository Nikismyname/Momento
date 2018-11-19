namespace Momento.Models.ListsRemind
{
    using Momento.Models.Contracts;
    using Momento.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ListRemind : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public ListRemind()
        {
            this.Items = new HashSet<ListRemindItem>();
            this.CreatedOn = DateTime.UtcNow;
        }

        public string  Name { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<ListRemindItem> Items { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
