using Momento.Services.Models.Admin;

namespace Momento.Services.Contracts.Admin
{
    public interface IAdminService
    {
        AdminViewUser[] GetAllUsers(string username);

        bool PromoteUser(string userId);

        bool DemoteUser(string userId);
    }
}
