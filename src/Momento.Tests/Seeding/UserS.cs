﻿namespace Momento.Tests.Seeding
{
    using Momento.Data;
    using Momento.Models.Directories;
    using Momento.Models.Users;

    public static class UserS
    {
        public static string PeshoId = "PeshoPeshovId";
        public static string PeshoUsername = "PeshoPeshov";
        public static string PeshoRootDir = "PeshoRoot";
        public static int PeshoRootDirId = 1;
        public static string PeshoPassword = "PeshoPass";
        public static string PeshoEmail = "Pesho@Pesho.Pesho";

        public static string GoshoId = "GoshoGoshovId";
        public static string GoshoUsername = "GoshoGoshov";
        public static string GoshoRootDir = "GoshoRoot";
        public static int GoshoRootDirId = 2;
        public static string GoshoPassword = "GoshoPass";
        public static string GoshoEmail = "Gosho@Gosho.Gosho";

        /// <summary>
        /// Also seeds their root directories!
        /// </summary>
        public static void SeedPeshoAndGosho(MomentoDbContext context)
        {
            var users = new User[]
            {
                new User
                {
                    Id = PeshoId,
                    FirstName = "Pesho",
                    LastName = "Peshov",
                    UserName = PeshoUsername,
                    Email = "pesho@pesho.pesho",
                    Directories = new Directory[]{ new Directory {Name = PeshoRootDir, Id = PeshoRootDirId  } }
                },
                new User
                {
                    Id = GoshoId,
                    FirstName = "Gosho",
                    LastName = "Goshov",
                    UserName = GoshoUsername,
                    Email = "gosho@gosho.gosho",
                    Directories = new Directory[]{ new Directory {Name = GoshoRootDir, Id = GoshoRootDirId  } }
                },
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
