namespace Momento.Services.Contracts.Directory
{
    using Momento.Services.Models.DirectoryModels;

    public interface IDirectoryService
    {
        DirectoryIndex GetIndex(string username);

        DirectoryIndexSingle GetIndexSingle(int? directoryId, string username);

        DirectoryIndexSingle GetIndexSingleApi(int? directoryId, string username);

        int Create(int parentDirId, string dirName, string username);

        int CreateApi(int parentDirId, string dirName, string username);

        void CreateRoot(string username);

        void Delete(int id, string username);

        bool DeleteApi(int id, string username);
    }
}
