using System.ComponentModel.DataAnnotations;

namespace Momento.Services.Models.NoteModels
{
    public class NoteCreate
    {
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Note Name must be between 3 and 40 characters long!")]
        public string Name { get; set; }

        public string Description { get; set; }

        public int DirectoryId { get; set; }
    }
}
