namespace Momento.Services.Models.ComparisonModels
{
    using Momento.Models.Comparisons;
    using Momento.Services.Models.ComparisonModels;
    using Momento.Services.Models.Contracts;
    using System.Collections.Generic;

    public class ComparisonEdit : IMapFrom<Comparison>
    {
        public ComparisonEdit()
        {
            this.Items = new HashSet<ComparisonItemEdit>();
        }

        public int Id { get; set; }

        public int DirectoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SourceLanguage { get; set; }

        public string TargetLanguage { get; set; }

        public int Order { get; set; }

        public HashSet<ComparisonItemEdit> Items { get; set; }
    }
}
