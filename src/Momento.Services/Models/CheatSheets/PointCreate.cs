namespace Momento.Services.Models.CheatSheets
{
    using Momento.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class PointCreate
    {
        public int SheetId { get; set; }
        public int TopicId { get; set; }

        public string  Name { get; set; }

        [MinLength(3), Required]
        public string Content { get; set; }

        public Formatting Formatting { get; set; }
    }
}
