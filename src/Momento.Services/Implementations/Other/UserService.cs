namespace Momento.Services.Implementations.Other
{
    using Momento.Data;
    using Momento.Models.Users;
    using Momento.Services.Contracts.Other;
    using Momento.Services.Exceptions;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly MomentoDbContext context;

        public UserService(MomentoDbContext context)
        {
            this.context = context;
        }

        public User ByUsername (string username)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == username);

            if(user == null)
            {
                throw new UserNotFound(username); 
            }

            return user;
        }

        public string GetUserId(string username)
        {
            var userId = context.Users.Where(x => x.UserName == username).SingleOrDefault()?.Id;

            if (userId == null)
            {
                throw new UserNotFound(username);
            }

            return userId;
        }
    }
}
