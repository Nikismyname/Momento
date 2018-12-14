namespace Momento.Services.Models.ComparisonModels
{
    using Momento.Services.Models.Attributes;
    using System.Collections.Generic;

    public class ComparisonSave
    {
        public ComparisonSave()
        {
            this.NewItems = new HashSet<ComparisonItemEdit>();
            this.AlteredItems = new HashSet<ComparisonItemChange>();
        }
        
        public int Id { get; set; }

        [ComparisonProperty]
        public string Name { get; set; }

        [ComparisonProperty]
        public string Description { get; set; }

        [ComparisonProperty]
        public string SourceLanguage { get; set; }

        [ComparisonProperty]
        public string TargetLanguage { get; set; }

        public HashSet<ComparisonItemEdit> NewItems { get; set; }

        public HashSet<ComparisonItemChange> AlteredItems { get; set; }
    }
}
