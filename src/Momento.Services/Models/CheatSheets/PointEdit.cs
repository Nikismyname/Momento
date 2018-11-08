namespace Momento.Services.Models.CheatSheets
{
    using Momento.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class PointEdit
    {
        public int Id { get; set; }

        [MinLength(3), Required]
        public string Name { get; set; }

        [MinLength(3), Required]
        public string Content { get; set; }

        public Formatting? Formatting { get; set; }
    }
}
