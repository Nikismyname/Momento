namespace Momento.Models.Contracts
{
    using Momento.Models.Hashtags;

    public interface HashtagMappingClass
    {
        int HashtagId { get; set; }
        Hashtag Hashtag { get; set; }
    }
}
