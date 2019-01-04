namespace Momento.Services.Models.VideoModels
{
    public class VideoInitialCreate
    {
        public int DirectoryId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
    }
}
