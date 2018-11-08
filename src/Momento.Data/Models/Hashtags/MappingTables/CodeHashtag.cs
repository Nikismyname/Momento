namespace Momento.Data.Models.Hashtags.MappingTables
{
    using Momento.Data.Models.Codes;
    using Momento.Data.Models.Contracts;

    public class CodeHashtag : HashtagMappingClass
    {
        public int CodeId { get; set; }
        public virtual Code Code { get; set; }

        public int  HashtagId { get; set; }
        public virtual Hashtag Hashtag { get; set; }
    }
}
