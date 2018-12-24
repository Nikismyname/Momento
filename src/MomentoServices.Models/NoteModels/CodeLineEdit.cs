namespace Momento.Services.Models.NoteModels
{
    using Momento.Models.Notes;
    using Momento.Services.Models.Contracts;

    public class CodeLineEdit : IMapFrom<CodeLine>, IMapTo<CodeLine>
    {
        public int Id { get; set; }

        public string SourceContent { get; set; }

        public int InPageId { get; set; }

        public int Order { get; set; }

        ///Note 
        public string NoteContent { get; set; }

        public bool EditorMode { get; set; }

        public bool Visible { get; set; }
        ///...
    }
}
