namespace Momento.Data.Models.Hashtags.MappingTables
{
    using Momento.Data.Models.Codes;
    using Momento.Data.Models.Contracts;
    using Momento.Data.Models.Hashtags;

    public class CodeNoteHashtag : HashtagMappingClass
    {
        public int  CodeNoteId { get; set; }
        public virtual CodeNote CodeNote { get; set; }

        public int  HashtagId  { get; set; }
        public virtual Hashtag Hashtag { get; set; }
    }
}
