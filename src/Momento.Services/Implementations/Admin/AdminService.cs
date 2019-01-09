namespace Momento.Services.Implementations.Admin
{
    using Microsoft.AspNetCore.Identity;
    using Data;
    using Momento.Models.Users;
    using Momento.Services.Contracts.Admin;
    using Mapping;
    using Momento.Services.Models.Admin;
    using System.Linq;

    public class AdminService : IAdminService
    {
        private readonly MomentoDbContext context;
        private readonly UserManager<User> userManager;

        public AdminService(MomentoDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public AdminViewUser[] GetAllUsers(string username)
        {
            var users = this.context.Users.To<AdminViewUser>().ToArray();

            var adminRoleId = context.Roles.SingleOrDefault(x => x.Name == "Admin").Id;
            for (var i = 0; i < users.Length; i++)
            {
                users[i].IsAdmin = this.context.UserRoles
                        .Where(x => x.UserId == users[i].Id)
                        .Where(x => x.RoleId == adminRoleId)
                        .Count() > 0? true : false;
                users[i].IsCurrentUser = users[i].UserName == username ? true : false;
            }

            return users;
        }

        public bool DemoteUser(string userId)
        {
            try
            {
                var user = context.Users.SingleOrDefault(x => x.Id == userId);
                userManager.RemoveFromRoleAsync(user, "Admin").GetAwaiter().GetResult();
                userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public bool PromoteUser(string userId)
        {
            try
            {
                var user = context.Users.SingleOrDefault(x => x.Id == userId);
                userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
