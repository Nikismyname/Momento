namespace Momento.Data.Models.Contracts
{
    using Momento.Data.Models.Hashtags;

    public interface HashtagMappingClass
    {
        int HashtagId { get; set; }
        Hashtag Hashtag { get; set; }
    }
}
