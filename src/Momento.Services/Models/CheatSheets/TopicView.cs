namespace Momento.Services.Models.CheatSheets
{
    using System.Collections.Generic;

    public class TopicView
    {
        public TopicView()
        {
            Points = new HashSet<PointPreview>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<PointPreview> Points { get; set; }
    }
}
