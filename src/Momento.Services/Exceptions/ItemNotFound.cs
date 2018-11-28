namespace Momento.Services.Exceptions
{
    using System;

    public class ItemNotFound : Exception
    {
        private const string CustomMessage = "{0} with id [{1}] does not exist!";

        public ItemNotFound(string itemType, string itemId) : 
            base(string.Format(CustomMessage,itemType,itemId)) { }

        public ItemNotFound(string message) : base(message) { }
    }
}
