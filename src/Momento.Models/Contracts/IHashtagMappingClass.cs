namespace Momento.Models.Contracts
{
    using Momento.Models.Hashtags;

    public interface IHashtagMappingClass
    {
        int HashtagId { get; set; }
        Hashtag Hashtag { get; set; }
    }
}
