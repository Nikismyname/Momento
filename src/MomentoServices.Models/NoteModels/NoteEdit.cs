namespace Momento.Services.Models.NoteModels
{
    using Momento.Models.Notes;
    using Momento.Services.Models.Contracts;
    using System.Collections.Generic;

    public class NoteEdit : IMapFrom<Note>
    {
        public NoteEdit()
        {
            this.Lines = new HashSet<CodeLineEdit>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        ///Main Note
        public string MainNoteContent { get; set; }

        public bool EditorMode { get; set; }
        ///...

        ///Code Optional
        public string Source { get; set; }

        public bool ShowSourceEditor { get; set; }

        public virtual ICollection<CodeLineEdit> Lines { get; set; }
        ///...
    }
}
