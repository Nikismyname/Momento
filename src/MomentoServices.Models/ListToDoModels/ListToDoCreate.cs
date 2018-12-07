namespace Momento.Services.Models.ListToDoModels
{
    using System.ComponentModel.DataAnnotations;

    public class ListToDoCreate
    {
        public int DirectoryId { get; set; }

        public int? Id { get; set; }

        public string Name { get; set; }

        public string Desctiption { get; set; }
    }
}
