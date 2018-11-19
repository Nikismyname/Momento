namespace Momento.Models.ListsRemind
{
    using Momento.Models.Contracts;
    using Momento.Models.Enums;
    using System;

    public class ListRemindItem : SoftDeletableAndTrackable
    {
        public ListRemindItem()
        {
            this.Importance = 1;
            this.Status = ListItemStatus.Remember;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public ListItemStatus Status { get; set; }

        public int Importance { get; set; }

        public int  ListId { get; set; }
        public virtual ListRemind List { get; set; }
    }
}
