namespace Momento.Services.Models.DirectoryModels
{
    using Momento.Services.Models.ListToDoModels;
    using Momento.Services.Models.VideoModels;
    using System.Collections.Generic;

    public class DirectoryIndex
    {
        public int  Id { get; set; }

        public int ParentDirectoryId { get; set; }

        public int?  CurrentDirId { get; set; }

        public string  Name { get; set; }

        public int Order { get; set; }

        public ICollection<VideoIndex> Videos { get; set; }

        public ICollection<ListToDoIndex> ListsToDo { get; set; }

        public ICollection<DirectoryIndex> Subdirectories { get; set; }
    }
}
