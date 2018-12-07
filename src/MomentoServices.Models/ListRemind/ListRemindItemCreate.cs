namespace Momento.Services.Models.ListRemind
{
    using System.ComponentModel.DataAnnotations;

    public class ListRemindItemCreate
    {
        public ListRemindItemCreate()
        {
            this.Importance = 1;
        }

        public string Content { get; set; }

        [Range(1,10)]
        public int Importance { get; set; }

        public int  InPageId { get; set; }
    }
}
