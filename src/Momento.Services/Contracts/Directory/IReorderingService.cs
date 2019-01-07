namespace Momento.Services.Contracts.Directory
{
    public interface IReorderingService
    {
        void Reorder(string type, int dir, int[][] ItemIdNewOrderKVP, string username);
    }
}
