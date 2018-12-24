namespace Momento.Models.Notes
{
    using Momento.Models.Contracts;

    public class CodeLine: SoftDeletableAndTrackable
    {
        const string defaultNoteContent= "<span style = \"color: rgb(255, 255, 255);\" >Double Click To Edit!</span>";

        public CodeLine()
        {
            this.NoteContent = defaultNoteContent;
            EditorMode = false; 
            Visible = true;
        }

        public int Id { get; set; }

        public string  SourceContent { get; set; }

        public int InPageId { get; set; }

        public int Order { get; set; }

        ///Note 
        public string  NoteContent { get; set; }

        public bool EditorMode { get; set; }

        public bool Visible { get; set; }
        ///...

        public int NoteId { get; set; }
        public virtual Note Note { get; set; }
    }
}
