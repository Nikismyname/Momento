namespace MomentoServices.Models.DirectoryModels
{
    using Momento.Models.Directories;
    using Momento.Services.Models.Contracts;

    public class DirectoryNavigation : IMapFrom<Directory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
