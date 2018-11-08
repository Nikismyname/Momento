namespace Momento.Services.Models.CheatSheets
{
    using System.ComponentModel.DataAnnotations;

    public class TopicCreate
    {
        public int SheetId { get; set; }

        [StringLength(30, MinimumLength = 3), Required]
        public string Name { get; set; }
    }
}
