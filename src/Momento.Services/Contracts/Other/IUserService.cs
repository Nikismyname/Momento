namespace Momento.Services.Contracts.Other
{
    using Momento.Models.Users;

    public interface IUserService
    {
        User ByUsername(string username);

        string GetUserId(string username);
    }
}
