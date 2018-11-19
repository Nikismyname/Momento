namespace Momento.Services.Contracts.Home
{
    using Momento.Services.Models.Home;

    public interface IHomeService
    {
        HomeIndex GetIndexData();
    }
}
