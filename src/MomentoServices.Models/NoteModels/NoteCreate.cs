namespace Momento.Services.Models.NoteModels
{
    public class NoteCreate
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int DirectoryId { get; set; }
    }
}
