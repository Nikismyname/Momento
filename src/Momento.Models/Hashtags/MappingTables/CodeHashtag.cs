namespace Momento.Models.Hashtags.MappingTables
{
    using Momento.Models.Codes;
    using Momento.Models.Contracts;

    public class CodeHashtag : IHashtagMappingClass
    {
        public int CodeId { get; set; }
        public virtual Code Code { get; set; }

        public int  HashtagId { get; set; }
        public virtual Hashtag Hashtag { get; set; }
    }
}
