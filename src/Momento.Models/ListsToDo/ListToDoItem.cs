using Momento.Models.Contracts;
using System;

namespace Momento.Models.ListsToDo
{
    public class ListToDoItem : SoftDeletableAndTrackable
    {
        public ListToDoItem()
        {
        }

        public int Id { get; set; }

        public int ListToDoId  { get; set; }
        public virtual ListToDo ListToDo { get; set; }

        public string Content { get; set; }

        public string Comment { get; set; }

        public string Status { get; set; }

        public int Order { get; set; }
    }
}
