namespace Momento.Services.Models.Admin
{
    using AutoMapper;
    using Momento.Models.Users;
    using Momento.Services.Mapping.Contracts;
    using Momento.Services.Models.Contracts;
    using System.Linq;

    public class AdminViewUser : IMapFrom<User>, IHaveCustomMappings
    {
        public AdminViewUser()
        {
            this.IsAdmin = false;
        }

        public string Id { get; set; }

        public int RootDirectoryId { get; set; }

        ///NotAutomapped
        public bool IsAdmin { get; set; }

        public string UserName { get; set; }

        public int DirectoriesCount { get; set; }

        public int VideosCount { get; set; }

        public int ListsToDoCount { get; set; }

        public int NotesCount { get; set; }

        public int ComparisonsCount { get; set; }

        public bool IsCurrentUser { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<User, AdminViewUser>()
                .ForMember(src => src.RootDirectoryId,
                    opt => opt.MapFrom(dest => dest.Directories
                        .Where(x => x.Name == "Root")
                        .OrderByDescending(x=>x.Subdirectories.Count)
                        .First()
                        .Id));
        }
    }
}
