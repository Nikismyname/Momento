namespace Momento.Services.Models.Admin
{
    public class AdminViewUser
    {
        public string Usename { get; set; }

        public int FoldersCount { get; set; }

        public int VidesCount { get; set; }

        public int ListToDoCount { get; set; }

        public int RootDirectoryId { get; set; }
    }
}
