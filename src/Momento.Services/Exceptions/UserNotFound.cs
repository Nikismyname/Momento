namespace Momento.Services.Exceptions
{
    using System;

    public class UserNotFound: Exception
    {
        private const string CustomMessage = "User with username [{0}] not found!";

        public UserNotFound(string username) :
            base(string.Format(CustomMessage, username)) { }
    }
}
