using System.ComponentModel.DataAnnotations;

namespace Momento.Services.Models.ComparisonModels
{
    public class ComparisonCreate
    {
        public int ParentDirId { get; set; }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "The comparison Name should be between 3 and 40 characters")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string SourceLanguage { get; set; }

        [Required]
        public string TargetLanguage { get; set; }
    }
}
