namespace Momento.Services.Models.Code
{
    using Momento.Models.Enums;

    public class CodeNoteCreate
    {
        public string Content { get; set; }

        public int WordId { get; set; }
        
        public Formatting Formatting { get; set; }

        public string Hashtags { get; set; }

        public int InPageId { get; set; }

        public int Level { get; set; }

        public bool Deleted { get; set; }
    }
}
