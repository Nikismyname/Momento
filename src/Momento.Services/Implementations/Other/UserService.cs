namespace Momento.Services.Implementations.Other
{
    using Momento.Data;
    using Momento.Data.Models.Users;
    using Momento.Services.Contracts.Other;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly MomentoDbContext context;

        public UserService(MomentoDbContext context)
        {
            this.context = context;
        }

        public User ByEmail (string email)
        {
            var user = context.Users.SingleOrDefault(x => x.Email == email);
            return user;
        }
    }
}
