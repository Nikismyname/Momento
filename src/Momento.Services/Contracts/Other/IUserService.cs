namespace Momento.Services.Contracts.Other
{
    using Momento.Models.Users;

    public interface IUserService
    {
        User ByEmail(string email);
    }
}
