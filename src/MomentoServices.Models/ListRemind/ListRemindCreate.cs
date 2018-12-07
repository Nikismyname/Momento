namespace Momento.Services.Models.ListRemind
{
    using System.Collections.Generic;

    public class ListRemindCreate
    {
        public ListRemindCreate()
        {
            ListItems = new List<ListRemindItemCreate>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<ListRemindItemCreate> ListItems { get; set; }
    }
}
