namespace Momento.Services.Contracts.Directory
{
    using Momento.Services.Models.DirectoryModels;
    using MomentoServices.Models.DirectoryModels;

    public interface IDirectoryService
    {
        DirectoryIndex GetIndex(string username);

        DirectoryImdexSingle GetIndexSingle(int? directoryId, string username);

        void Create(int id, string name, string username);

        void CreateRoot(string username);

        void Delete(int id);
    }
}
