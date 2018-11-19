namespace Momento.Models.Hashtags.MappingTables
{
    using Momento.Models.Codes;
    using Momento.Models.Contracts;
    using Momento.Models.Hashtags;

    public class CodeNoteHashtag : IHashtagMappingClass
    {
        public int  CodeNoteId { get; set; }
        public virtual CodeNote CodeNote { get; set; }

        public int  HashtagId  { get; set; }
        public virtual Hashtag Hashtag { get; set; }
    }
}
