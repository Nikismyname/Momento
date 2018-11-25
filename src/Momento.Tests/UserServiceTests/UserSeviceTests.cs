namespace Momento.Tests.UserServiceTests
{
    using NUnit.Framework;
    using FluentAssertions;
    using Momento.Data;
    using Momento.Services.Contracts.Other;
    using Momento.Services.Implementations.Other;
    using Microsoft.EntityFrameworkCore;
    using Momento.Models.Users;
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using System.Collections.Generic;
    using Momento.Services.Exceptions;

    ///Do not use Equals, use be or equal

    ///TODO: Extract repeating functionallity to parent class
    [TestFixture]
    public class UserSeviceTests
    {
        private MomentoDbContext context;
        private IUserService userService;

        [SetUp]
        protected void Setup()
        {
            var options = new DbContextOptionsBuilder<MomentoDbContext>()
                ///If you name the database the same thing it will be reused
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            context = new MomentoDbContext(options);
            context.Database.EnsureCreated();
            this.Seed(context);
            this.userService = new UserService(context);
        } 

        [TearDown]
        protected void Dospose()
        {
            ///So databases are not stored in memory
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public void GetUserIdThrowsIfUserNotFound()
        {
            var username = "NonExistantUsername";
            Action action = () => this.userService.GetUserId(username);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void GetUserIdShouldReturnTheRightIdIfUserExists()
        {
            var username = "GoshoGoshev";
            var correctId = "GoshoGoshevId";
            var id = userService.GetUserId(username);
            id.Should().Be(correctId);
        }

        [Test]
        public void ByUsernameShouldThrowIfUserIsNotFound()
        {
            var username = "NonExistantUsername";
            Action action = () => userService.ByUsername(username);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void ByUsernameShouldReturnTheRightUser()
        {
            var username = "PeshoPeshov";
            var correctId = "PeshoPeshovId";
            var user = userService.ByUsername(username);
            user.Id.Should().Be(correctId);
        } 


        private void Seed(MomentoDbContext context)
        {
            var users = new User[]
            {
                new User
                {
                    Id = "PeshoPeshovId",
                    FirstName = "Pesho",
                    LastName = "Peshov",
                    UserName = "PeshoPeshov",
                    Email = "pesho@pesho.pesho",
                },
                new User
                {
                    Id = "GoshoGoshevId",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    UserName = "GoshoGoshev",
                    Email = "gosho@gosho.gosho",
                },
            };

            //var userStore = new UserStore(new UserRepository(), roleRepository, mediator, clock);
            //IPasswordHasher<User> hasher = new PasswordHasher<User>();
            //var validator = new UserValidator<User>();
            //var validators = new List<UserValidator<User>> { validator };
            //var userManager = new UserManager<User>(userStore, null, hasher, validators, null, null, null, null, null);

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
