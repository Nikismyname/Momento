namespace Momento.Services.Models.VideoModels
{
    using System.Collections.Generic;

    public class VideoNoteMouseOver
    {
        public VideoNoteMouseOver()
        {
            this.ChildNotes = new HashSet<VideoNoteMouseOver>();
        }

        public bool IsRoot { get; set; }

        public string  FormattedPreview { get; set; }

        public ICollection<VideoNoteMouseOver> ChildNotes { get; set; }
    }
}
