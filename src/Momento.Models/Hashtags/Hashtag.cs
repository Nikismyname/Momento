namespace Momento.Models.Hashtags
{
    using Momento.Models.Hashtags.MappingTables;
    using System.Collections.Generic;

    public class Hashtag
    {
        public Hashtag()
        {
            this.CodeHashtags = new HashSet<CodeHashtag>();
            this.CodeNoteHashtags = new HashSet<CodeNoteHashtag>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<CodeHashtag> CodeHashtags { get; set; }

        public virtual ICollection<CodeNoteHashtag> CodeNoteHashtags { get; set; }
    }
}
