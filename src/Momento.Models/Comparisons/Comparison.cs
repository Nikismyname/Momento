namespace Momento.Models.Comparisons
{
    using Momento.Models.Contracts;
    using Momento.Models.Directories;
    using Momento.Models.Users;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Comparison : SoftDeletableAndTrackable
    {
        public Comparison()
        {
            this.Items = new HashSet<ComparisonItem>();
        }

        public int Id { get; set; }

        public int DirectoryId { get; set; }
        public virtual Directory Directory { get; set; }

        [Required]
        public string  UserId { get; set; }
        public virtual User User { get; set; }

        public string  Name { get; set; }

        public string  Description { get; set; }

        public string  SourceLanguage { get; set; }

        public string  TargetLanguage { get; set; }

        /// <summary>
        /// 0 based
        /// </summary>
        public int  Order { get; set; }

        public virtual ICollection<ComparisonItem> Items { get; set; }
    }
}
