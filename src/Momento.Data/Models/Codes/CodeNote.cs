namespace Momento.Data.Models.Codes
{
    using Momento.Data.Models.Contracts;
    using Momento.Data.Models.Enums;
    using Momento.Data.Models.Hashtags.MappingTables;
    using System;
    using System.Collections.Generic;

    public class CodeNote : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public CodeNote()
        {
            this.CodeNoteHashtags = new HashSet<CodeNoteHashtag>();
            this.IsDeleted = false;
        }

        public string Content { get; set; }

        public int WordId { get; set; }

        public Formatting Formatting { get; set; }

        public int  Order { get; set; }

        public int CodeSnippetId { get; set; }
        public virtual Code CodeSnippet { get; set; }

        public virtual ICollection<CodeNoteHashtag> CodeNoteHashtags { get; set; }

        public string Hashtags { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
