namespace Momento.Web.Profiles
{
    using AutoMapper;
    using Momento.Models.ListsToDo;
    using Momento.Models.Videos;
    using Momento.Models.Directories;
    using Momento.Services.Models.VideoModels;
    using Momento.Services.Models.ListToDoModels;
    using Momento.Services.Models.DirectoryModels;

    public class MomentoProfile: Profile
    {
        public MomentoProfile()
        {
            CreateMap<VideoNote, VideoNoteMouseOver>()
                .ForMember(dest => dest.IsRoot,
                            opt=> opt.MapFrom(src=>src.NoteId == null? true : false ));
                            
            CreateMap<Video, VideoIndex>()
                .ForMember(dest => dest.NotesCount,
                            src => src.MapFrom(x => x.Notes.Count));

            CreateMap<ListToDo, ListToDoIndex>();

            CreateMap<Directory, DirectoryIndex>();

            CreateMap<ListToDoCreate, ListToDo>().ReverseMap();

            CreateMap<ListToDoItem, ListToDoItemUse>().ReverseMap();

            CreateMap<ListToDo, ListToDoUse>().ReverseMap();
        }
    }
}
