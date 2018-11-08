namespace Momento.Services.Contracts.Directory
{
    using Momento.Services.Models.Video;

    public interface IDirectoryService
    {
        DirectoryIndex GetIndex(string userName);

        void Create(int id, string name, string userName);

        void CreateRoot(string userName);

        void Delete(int id);
    }
}
