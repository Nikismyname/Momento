namespace Momento.Tests.Seeding
{
    using Momento.Data;
    using Momento.Models.Directories;
    using System.Linq;

    public static class DirS
    {
        public const string DefaultDirName = "ChildDir";

        public static Directory SeedDirectoryToDirectory(
            MomentoDbContext context, int parentDirId, int newDirId = 0, string name = DefaultDirName)
        {
            var parentDir = context.Directories.Single(x => x.Id == parentDirId);

            var directory = new Directory
            {
                Id = newDirId,
                Name = name,
                UserId = parentDir.UserId,
            };

            parentDir.Subdirectories.Add(directory);

            context.SaveChanges();

            return directory;
        }
    }
}
