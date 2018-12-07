namespace Momento.Services.Models.VideoModels
{
    using Momento.Models.Videos;
    using Momento.Services.Models.Contracts;
    using System.Collections.Generic;

    public class VideoIndex : IMapFrom<Video>
    {
        //public VideoIndex()
        //{
        //    this.Notes = new HashSet<VideoNoteMouseOver>();
        //}

        public int Id { get; set; }

        public string  Name { get; set; }

        public string Description { get; set; }

        public int  NotesCount { get; set; }

        public int Order { get; set; }

        //public ICollection<VideoNoteMouseOver> Notes { get; set; }
    }
}
