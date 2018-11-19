namespace Momento.Models.CheatSheets
{
    using Momento.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Topic : SoftDeletableAndTrackable
    {
        public Topic()
        {
            Points = new HashSet<Point>();
        }

        public int Id { get; set; }

        [StringLength(30, MinimumLength =3),Required]
        public string Name { get; set; }

        [Required]
        public int  CheatSheetId { get; set; }
        public virtual CheatSheet CheatSheet { get; set; }

        public virtual ICollection<Point> Points { get; set; }
    }
}
