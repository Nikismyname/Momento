namespace Momento.Services.Models.List
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ListCreate
    {
        public ListCreate()
        {
            ListItems = new List<ListItemCreate>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<ListItemCreate> ListItems { get; set; }
    }
}
