namespace Momento.Services.Models.ListToDoModels
{
    using Momento.Models.ListsToDo;
    using Momento.Services.Models.Contracts;

    public class ListToDoItemUse : IMapFrom<ListToDoItem>
    {
        public string Content { get; set; }

        public string Comment { get; set; }

        public string Status { get; set; }

        public int  Order { get; set; }

        public bool Deleted { get; set; }

        public int Id { get; set; }

        public bool Changed { get; set; }
    }
}
