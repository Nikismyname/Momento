namespace Momento.Models.CheatSheets
{
    using Momento.Models.Contracts;
    using Momento.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Point : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public Point()
        {
            this.ChildPoints = new HashSet<Point>();
            this.IsDeleted = false;
        }

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

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
