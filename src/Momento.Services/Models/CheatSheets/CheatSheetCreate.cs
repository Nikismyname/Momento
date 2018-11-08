namespace Momento.Services.Models.CheatSheets
{
    using System.ComponentModel.DataAnnotations;

    public class CheatSheetCreate
    {
        public int? Id { get; set; }
        [StringLength(30 , MinimumLength = 3),Required]
        public string Title { get; set; }
        [StringLength(300,MinimumLength = 3),Required]
        public string  Description { get; set; }
    }
}
