namespace Momento.Services.Exceptions
{
    using System;

    public class NoRightToAccessItem : Exception 
    {
        private const string CustomMessage = "You have no right to access this {0}!";

        public NoRightToAccessItem(string itemType) : 
            base(string.Format(CustomMessage,itemType)) { }
    }
}
