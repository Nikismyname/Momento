using Momento.Models.Contracts;
using System;

namespace Momento.Models.ListsToDo
{
    public class ListToDoItem : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public ListToDoItem()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public int ListToDoId  { get; set; }
        public virtual ListToDo ListToDo { get; set; }

        public string Content { get; set; }

        public string Comment { get; set; }

        public string Status { get; set; }

        public int Order { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
