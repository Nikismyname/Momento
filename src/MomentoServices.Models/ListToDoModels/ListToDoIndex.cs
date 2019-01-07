namespace Momento.Services.Models.ListToDoModels
{
    using Momento.Models.ListsToDo;
    using Momento.Services.Models.Contracts;

    public class ListToDoIndex : IMapFrom<ListToDo>
    {
        public string Id { get; set; }

        public string  Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
    }
} 
