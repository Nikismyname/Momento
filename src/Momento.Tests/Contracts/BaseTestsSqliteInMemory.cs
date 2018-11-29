namespace Momento.Tests.Contracts
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using NUnit.Framework;

    [TestFixture]
    public abstract class BaseTestsSqliteInMemory
    {
        protected MomentoDbContext context;

        [SetUp]
        public virtual void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<MomentoDbContext>()
                .UseSqlite(connection)
                .Options;
            this.context = new MomentoDbContext(options);
            context.Database.EnsureCreated();
        }

        [TearDown]
        public void Dospose()
        {
            ///So databases are not stored in memory
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
