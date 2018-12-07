namespace Momento.Services.Models.CheatSheets
{
    using System.ComponentModel.DataAnnotations;

    public class TopicEdit
    {
        public int  TopicId { get; set; }

        public int SheetId { get; set; }

        [StringLength(30, MinimumLength = 3), Required]
        public string Name { get; set; }
    }
}
