namespace Momento.Services.Models.ComparisonModels
{
    public class ComparisonCreate
    {
        public int ParentDirId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
    }
}
