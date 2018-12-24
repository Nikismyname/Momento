namespace Momento.Models.Notes
{
    using Momento.Models.Contracts;
    using Momento.Models.Directories;
    using Momento.Models.Users;
    using System.Collections.Generic;

    public class Note : SoftDeletableAndTrackable, IOrderable<int>
    {
        const string defaultNoteContent = "<span style = \"color: rgb(255, 255, 255);\" > Double Click To Edit!</span>";

        public Note()
        {
            this.Lines = new HashSet<CodeLine>();
            this.MainNoteContent = defaultNoteContent;
            this.EditorMode = false;

            this.Source = "";
            ShowSourceEditor = false;
        }

        public int Id { get; set; }

        public string  Name { get; set; }

        public string  Description { get; set; }

        public int Order { get; set; }

        ///Main Note
        public string  MainNoteContent { get; set; }

        public bool EditorMode { get; set; }
        ///...

        ///Code Optional
        public string  Source { get; set; }

        public bool ShowSourceEditor { get; set; }

        public virtual ICollection<CodeLine> Lines { get; set; }
        ///...

        public string  UserId { get; set; }
        public virtual User User { get; set; }

        public int  DirectoryId { get; set; }
        public virtual Directory Directory { get; set; }
    }
}
