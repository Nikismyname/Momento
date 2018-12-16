namespace Momento.Services.Contracts.Directory
{
    using Momento.Models.Contracts;
    using System.Linq;

    public interface IReorderingService
    {
        void SaveItemsForOneDir(int parentDir, string cntOrDir, int[] orderings);
    }
}
