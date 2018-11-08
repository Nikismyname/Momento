namespace Momento.Services.Models.ListToDoModels
{
    using System.Collections.Generic;

    public class ListToDoUse
    {
        public ListToDoUse()
        {
            this.Items = new List<ListToDoItemUse>();
        }

        public int Id { get; set; }

        public int  DirectoryId { get; set; }

        public string Name { get; set; }

        public string  Description { get; set; }

        public string Categories { get; set; }

        public List<ListToDoItemUse> Items { get; set; }
    }
}
