namespace Momento.Services.Exceptions
{
    using System;

    public class AccessDenied : Exception 
    {
        private const string CustomMessage = "You have no right to access this {0} with Id: {1}!";

        public AccessDenied(string itemType, int itemId) : 
            base(string.Format(CustomMessage,itemType, itemId)) { }

        public AccessDenied(string message) : base(message){ }
    }
}
