namespace Momento.Services.Models.VideoModels
{
    using AutoMapper;
    using Momento.Models.Videos;
    using Momento.Services.Mapping.Contracts;
    using Momento.Services.Models.Contracts;
    using System.Collections.Generic;
    using System.Linq;

    public class VideoView: IMapFrom<Video>, IHaveCustomMappings
    {
        public VideoView()
        {
            this.Notes = new List<VideoNoteView>();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public List<VideoNoteView> Notes { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Video, VideoView>()
                .ForMember(dest => dest.Notes,
                           opt => opt.MapFrom(src => src.Notes.Where(x => x.NoteId == null)));
        }
    }
}
