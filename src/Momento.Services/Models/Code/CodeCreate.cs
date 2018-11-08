namespace Momento.Services.Models.Code
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CodeCreate
    {
        public CodeCreate()
        {
            Notes = new HashSet<CodeNoteCreate>();
        }

        public int? Id { get; set; }

        public int DirectoryId { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }

        public string Hashtag { get; set; }

        public ICollection<CodeNoteCreate> Notes { get; set; }
    }
}
