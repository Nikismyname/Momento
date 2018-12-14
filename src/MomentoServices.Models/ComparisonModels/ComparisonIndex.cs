namespace Momento.Services.Models.ComparisonModels
{
    using Momento.Models.Comparisons;
    using Momento.Services.Models.Contracts;

    public class ComparisonIndex: IMapFrom<Comparison>
    {
        public int Id { get; set; }

        public string  Name { get; set; }

        public int ItemsCount { get; set; }
    }
}
