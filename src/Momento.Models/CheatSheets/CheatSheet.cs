namespace Momento.Models.CheatSheets
{
    using Momento.Models.Contracts;
    using Momento.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CheatSheet : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public CheatSheet()
        {
            Topics = new HashSet<Topic>();
        }

        [StringLength(30, MinimumLength = 3),Required]
        public string Name { get; set; }

        [StringLength(300, MinimumLength = 3),Required]
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Topic> Topics {get;set;}

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
