namespace Momento.Models.CheatSheets
{
    using Momento.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Topic : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public Topic()
        {
            Points = new HashSet<Point>();
            this.CreatedOn = DateTime.UtcNow;
        }

        [StringLength(30, MinimumLength =3),Required]
        public string Name { get; set; }

        [Required]
        public int  CheatSheetId { get; set; }
        public virtual CheatSheet CheatSheet { get; set; }

        public virtual ICollection<Point> Points { get; set; }


        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
