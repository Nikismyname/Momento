namespace Momento.Tests.Contracts
{
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public abstract class BaseTestsMsInMemor
    {
        protected MomentoDbContext context;

        [SetUp]
        public virtual void Setup()
        {
            var options = new DbContextOptionsBuilder<MomentoDbContext>()
                ///If you name the database the same thing it will be reused
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            context = new MomentoDbContext(options);
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


///Attempt to Instantiate UserManager 
//var userStore = new UserStore(new UserRepository(), roleRepository, mediator, clock);
//IPasswordHasher<User> hasher = new PasswordHasher<User>();
//var validator = new UserValidator<User>();
//var validators = new List<UserValidator<User>> { validator };
//var userManager = new UserManager<User>(userStore, null, hasher, validators, null, null, null, null, null);

