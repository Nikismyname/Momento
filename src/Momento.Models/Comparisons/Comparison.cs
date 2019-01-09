namespace Momento.Models.Comparisons
{
    using Momento.Models.Contracts;
    using Momento.Models.Directories;
    using Momento.Models.Users;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Comparison : SoftDeletableAndTrackable, IOrderable<int>
    {
        public Comparison()
        {
            this.Items = new HashSet<ComparisonItem>();
        }

        public int Id { get; set; }

        /// <summary>
        /// 0 based
        /// </summary>
        public int Order { get; set; }

        public int DirectoryId { get; set; }
        public Directory Directory { get; set; }

        [Required]
        public string  UserId { get; set; }
        public User User { get; set; }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "The comparison Name should be between 3 and 40 characters")]
        public string  Name { get; set; }

        public string  Description { get; set; }

        [Required]
        public string  SourceLanguage { get; set; }

        [Required]
        public string  TargetLanguage { get; set; }

        public ICollection<ComparisonItem> Items { get; set; }
    }
}
