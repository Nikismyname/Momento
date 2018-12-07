namespace Momento.Services.Models.Home
{
    using Momento.Services.Models.ListToDoModels;
    using Momento.Services.Models.VideoModels;
    using System.Collections.Generic;

    public class HomeIndex
    {
        public IEnumerable<ListToDoIndex> ListsToDo { get; set; }
        public IEnumerable<VideoIndex> VideoNotes { get; set; }
        ///Add More
    }
}
