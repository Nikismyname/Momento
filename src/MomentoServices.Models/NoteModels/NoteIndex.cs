namespace Momento.Services.Models.NoteModels
{
    using Momento.Models.Notes;
    using Momento.Services.Models.Contracts;

    public class NoteIndex : IMapFrom<Note>
    {
        public int  Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int LinesCount { get; set; }

        public int Order { get; set; }
    }
}
