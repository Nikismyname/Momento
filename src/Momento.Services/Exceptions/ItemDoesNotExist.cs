namespace Momento.Services.Exceptions
{
    using System;

    public class ItemDoesNotExist : Exception
    {
        private const string CustomMessage = "{0} with id [{1}] does not exist!";

        public ItemDoesNotExist(string itemType, string itemId) : 
            base(string.Format(CustomMessage,itemType,itemId)) { }

        public ItemDoesNotExist(string message) : base(message) { }
    }
}
