namespace Momento.Services.Contracts.Other
{
    using Momento.Data.Models.Users;

    public interface IUserService
    {
        User ByEmail(string email);
    }
}
