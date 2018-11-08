using System.ComponentModel.DataAnnotations;

namespace Momento.Services.Models.List
{
    public class ListItemCreate
    {
        public ListItemCreate()
        {
            this.Importance = 1;
        }

        public string Content { get; set; }

        [Range(1,10)]
        public int Importance { get; set; }

        public int  InPageId { get; set; }
    }
}
