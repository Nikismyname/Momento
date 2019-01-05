namespace Momento.Services.Models.ListToDoModels
{
    using Momento.Models.ListsToDo;
    using Momento.Services.Models.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ListToDoUse: IMapFrom<ListToDo>
    {
        public ListToDoUse()
        {
            this.Items = new List<ListToDoItemUse>();
        }

        public int Id { get; set; }

        public int  DirectoryId { get; set; }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "The Name of a ListToDo must be between 3 and 40 characters long!")]
        public string Name { get; set; }

        public string  Description { get; set; }

        public string Categories { get; set; }

        public List<ListToDoItemUse> Items { get; set; }
    }
}
