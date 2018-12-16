namespace Momento.Services.Models.ListToDoModels
{
    using Momento.Models.ListsToDo;
    using Momento.Services.Models.Contracts;

    public class ListToDoCreate: IMapTo<ListToDo>
    {
        public int DirectoryId { get; set; }

        public string Name { get; set; }

        public string Desctiption { get; set; }
    }
}
