namespace Momento.Models.Codes
{
    using Momento.Models.Contracts;
    using Momento.Models.Enums;
    using Momento.Models.Hashtags.MappingTables;
    using System;
    using System.Collections.Generic;

    public class CodeNote : SoftDeletableAndTrackable
    {
        public CodeNote()
        {
            this.CodeNoteHashtags = new HashSet<CodeNoteHashtag>();
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public int WordId { get; set; }

        public Formatting Formatting { get; set; }

        public int  Order { get; set; }

        public int CodeSnippetId { get; set; }
        public virtual Code CodeSnippet { get; set; }

        public virtual ICollection<CodeNoteHashtag> CodeNoteHashtags { get; set; }

        public string Hashtags { get; set; }
    }
}
