namespace Momento.Models.Codes
{
    using Momento.Models.Contracts;
    using Momento.Models.Hashtags.MappingTables;
    using System;
    using System.Collections.Generic;

    public class Code : SoftDeletableAndTrackable
    {
        public Code()
        {
            this.Notes = new HashSet<CodeNote>();
            this.CodeHashtags = new HashSet<CodeHashtag>();
        }

        public int Id { get; set; }

        public int DirectoryId { get; set; }

        public string  Name { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }

        public virtual ICollection<CodeNote> Notes { get; set; }

        public virtual ICollection<CodeHashtag> CodeHashtags { get; set; }
    }
}
