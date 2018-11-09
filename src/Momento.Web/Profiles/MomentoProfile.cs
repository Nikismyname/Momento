namespace Momento.Web.Profiles
{
    using System.Linq;
    using AutoMapper;
    using Momento.Data.Models.Codes;
    using Momento.Data.Models.ListsToDo;
    using Momento.Data.Models.Videos;
    using Momento.Data.Models.Directories;
    using Momento.Services.Models.Code;
    using Momento.Services.Models.Video;
    using Momento.Services.Models.ListToDoModels;

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


            CreateMap<VideoNote, VideoNoteView>();

            CreateMap<Video, VideoView>()
                .ForMember(dest => dest.Notes,
                           opt => opt.MapFrom(src => src.Notes.Where(x=>x.NoteId==null)));


            CreateMap<Code, CodeCreate>().ReverseMap();

            CreateMap<CodeNote, CodeNoteCreate>().ReverseMap();


            CreateMap<ListToDoCreate, ListToDo>().ReverseMap();

            CreateMap<ListToDoItem, ListToDoItemUse>().ReverseMap();

            CreateMap<ListToDo, ListToDoUse>().ReverseMap();
        }
    }
}
