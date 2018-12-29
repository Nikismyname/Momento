namespace Momento.Tests.Utilities
{
    using FunApp.Web.Tests;
    using Microsoft.EntityFrameworkCore;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Web;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;
    using Xunit;

    public class SeleniumCreateTestData : SeleniumInMemoryDbBaseTest
    {
        public SeleniumCreateTestData(SeleniumServerFactoryInMemory<Startup> server) : base(server, false) { }

        ///Only Workd with lazy loading disabled
        //[Fact]
        public void CreateTextFileWithRegisteredPeshoAndGosho()
        {
            SeleniumActions.RegisterUser(UserS.PeshoUsername, UserS.PeshoEmail,
                UserS.PeshoPassword, this.Browser, this.RootUri);

            SeleniumActions.SubmitFormIfOnlyOnePresent(this.Browser);

            SeleniumActions.RegisterUser(UserS.GoshoUsername, UserS.GoshoEmail,
                UserS.GoshoPassword, this.Browser, this.RootUri);

            var users = this.Context.Users
                .Include(x => x.Directories)
                .Include(x => x.UserSettings)
                .ToArray();

            /// Removing the circular reference
            for (int i = 0; i < users.Length; i++)
            {
                users[i].UserSettings.User = null;
                foreach (var dir in users[i].Directories)
                {
                    dir.User = null;
                }
                users[i].Directories = users[i].Directories.ToHashSet();
            }

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("RegisteredUsers.txt", json);
        }
    }
}
