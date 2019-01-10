using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Momento.Tests.Seeding;

namespace Momento.Tests.Tests.SettingsServiceTests
{
    using System;
    using Contracts;
    using Momento.Services.Contracts.Other;
    using Services.Implementations.Other;
    using NUnit.Framework;
    using Models.Users;

    public class SettingsServiceTests: BaseTestsSqliteInMemory
    {
        private ISettingsService settingsService;

        public override void Setup()
        {
            base.Setup();
            this.settingsService = new SettingsService(this.context);
        }

        [Test]
        public void CreateSettingCreatesSettingsForTheGivenUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            Func<UserSettings> action = () => this.settingsService.CreateSettings(UserS.PeshoUsername);
            action.Invoke();
            var pesho = this.context.Users
                .Include(x => x.UserSettings)
                .SingleOrDefault(x=>x.UserName==UserS.PeshoUsername);

            var peshoSettings = pesho.UserSettings;
            peshoSettings.Should().NotBeNull();

        }
    }
}
