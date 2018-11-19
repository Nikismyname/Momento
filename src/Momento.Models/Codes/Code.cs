namespace Momento.Models.Codes
{
    using Momento.Models.Contracts;
    using Momento.Models.Hashtags.MappingTables;
    using System;
    using System.Collections.Generic;

    public class Code : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public Code()
        {
            this.Notes = new HashSet<CodeNote>();
            this.CodeHashtags = new HashSet<CodeHashtag>();
            this.IsDeleted = false;
            this.CreatedOn = DateTime.UtcNow;
        }

        public int DirectoryId { get; set; }

        public string  Name { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }

        public virtual ICollection<CodeNote> Notes { get; set; }

        public virtual ICollection<CodeHashtag> CodeHashtags { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
