namespace Momento.Services.Contracts.Directory
{
    public interface IReorderingService
    {
        //void AddOrdering(string cntOrDir, string user, int parentDirId, int[] ordering);

        //void SaveOrderingForUser(string user);

        //void ReorderElements(string cntOrDir, int parentDirId, int[] ordering);

        void SaveItemsForOneDir(int parentDir, string cntOrDir, int[] orderings);
    }
}
