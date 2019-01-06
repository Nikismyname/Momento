namespace Momento.Models.ListsToDo
{
    using Momento.Models.Contracts;
    using Momento.Models.Directories;
    using Momento.Models.Users;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ListToDo : SoftDeletableAndTrackable, IOrderable<int>
    {
        public const string InitialCategories = "HighPriority;Active;BackBurner;MostlyDone;Done;Unassigned";

        public ListToDo()
        {
            this.Items = new HashSet<ListToDoItem>();
            this.Categories = InitialCategories;
        }

        public int  Id { get; set; }

        public int Order { get; set; }

        public string  UserId { get; set; }
        public virtual User User { get; set; }

        public int  DirectoryId { get; set; }
        public virtual Directory Directory { get; set; }

        [StringLength(40,MinimumLength =3, ErrorMessage = "The Name of a ListToDo must be between 3 and 40 characters long!")]
        public string  Name { get; set; }

        public string  Description { get; set; }

        public string Categories { get; set; }

        public virtual ICollection<ListToDoItem> Items { get; set; }
    }
}
