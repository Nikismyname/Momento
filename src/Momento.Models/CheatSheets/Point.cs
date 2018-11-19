namespace Momento.Models.CheatSheets
{
    using Momento.Models.Contracts;
    using Momento.Models.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Point : SoftDeletableAndTrackable
    {
        public Point()
        {
            this.ChildPoints = new HashSet<Point>();
        }

        public int  Id { get; set; }

        [MinLength(3), Required]
        public string Name { get; set; }

        [MinLength(3),Required]
        public string  Content { get; set; }

        public Formatting Formatting { get; set; }

        public int? TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        public int? ParentPointId { get; set; }
        public virtual Point ParentPoint { get; set; }

        public virtual ICollection<Point> ChildPoints { get; set; }
    }
}
