namespace Momento.Services.Models.ListToDoModels
{
    using Momento.Models.ListsToDo;
    using Momento.Services.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class ListToDoCreate: IMapTo<ListToDo>
    {
        public int DirectoryId { get; set; }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "The Name of a ListToDo must be between 3 and 40 characters long!")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
