namespace MomentoServices.Models.DirectoryModels
{
    using Momento.Models.Directories;
    using Momento.Services.Models.Contracts;
    using Momento.Services.Models.ListToDoModels;
    using Momento.Services.Models.VideoModels;
    using System.Collections.Generic;

    public class DirectoryImdexSingle : IMapFrom<Directory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentDirectoryId { get; set; }

        public ICollection<VideoIndex> Videos { get; set; }

        public ICollection<ListToDoIndex> ListsToDo { get; set; }

        public ICollection<DirectoryNavigation> Subdirectories { get; set; }
    }
}
