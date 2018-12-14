namespace Momento.Services.Models.ComparisonModels
{
    using AutoMapper;
    using Momento.Models.Comparisons;
    using Momento.Services.Mapping.Contracts;
    using Momento.Services.Models.Contracts;

    public class ComparisonItemEdit : IMapFrom<ComparisonItem>, IMapTo<ComparisonItem>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

        public string Comment { get; set; }

        /// <summary>
        /// 0 based
        /// </summary>
        public int Order { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ComparisonItemEdit, ComparisonItem>()
                .ForMember(src => src.Id, opt => opt.MapFrom(dest => 0));
        }
    }
}
