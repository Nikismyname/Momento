namespace Momento.Services.Models.ListToDoModels
{
    public class ListToDoItemUse
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
