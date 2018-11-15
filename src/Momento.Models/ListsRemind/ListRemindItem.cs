namespace Momento.Models.ListsRemind
{
    using Momento.Models.Contracts;
    using Momento.Models.Enums;
    using System;

    public class ListRemindItem : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public ListRemindItem()
        {
            Importance = 1;
            Status = ListItemStatus.Remember;
        }

        public string Content { get; set; }

        public ListItemStatus Status { get; set; }

        public int Importance { get; set; }

        public int  ListId { get; set; }
        public virtual ListRemind List { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
