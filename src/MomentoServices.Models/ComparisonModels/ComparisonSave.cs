namespace Momento.Services.Models.ComparisonModels
{
    using Momento.Services.Models.Attributes;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ComparisonSave
    {
        public ComparisonSave()
        {
            this.NewItems = new HashSet<ComparisonItemEdit>();
            this.AlteredItems = new HashSet<ComparisonItemChange>();
        }
        
        public int Id { get; set; }

        [ComparisonProperty]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "The comparison Name should be between 3 and 40 characters")]
        public string Name { get; set; }

        [ComparisonProperty]
        public string Description { get; set; }

        [ComparisonProperty]
        [Required]
        public string SourceLanguage { get; set; }

        [ComparisonProperty]
        [Required]
        public string TargetLanguage { get; set; }

        public HashSet<ComparisonItemEdit> NewItems { get; set; }

        public HashSet<ComparisonItemChange> AlteredItems { get; set; }
    }
}
