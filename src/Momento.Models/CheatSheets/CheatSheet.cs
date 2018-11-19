namespace Momento.Models.CheatSheets
{
    using Momento.Models.Contracts;
    using Momento.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CheatSheet : SoftDeletableAndTrackable
    {
        public CheatSheet()
        {
            Topics = new HashSet<Topic>();
        }

        public int Id { get; set; }

        [StringLength(30, MinimumLength = 3),Required]
        public string Name { get; set; }

        [StringLength(300, MinimumLength = 3),Required]
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Topic> Topics {get;set;}
    }
}
