namespace Momento.Services.Contracts.Directory
{
    using Momento.Services.Models.DirectoryModels;

    public interface IDirectoryService
    {
        DirectoryIndexSingle GetIndexSingle(
            int? directoryId, string username, bool isAdmin = false);

        DirectoryIndexSingle GetIndexSingleApi(
            int? directoryId, string username, bool isAdmin = false);

        int Create(
            int parentDirId, string dirName, string username, 
            bool isAdmin = false);

        int CreateApi(
            int parentDirId, string dirName, string username, 
            bool isAdmin = false);

        Momento.Models.Directories.Directory Delete(
            int id, string username, bool isAdmin = false);

        bool DeleteApi(
            int id, string username, bool isAdmin = false);

        void CreateRoot(string username);

        ///Not in use///In use apparently///Not in user anymore 
        //DirectoryIndex GetIndex(string username);
    }
}
