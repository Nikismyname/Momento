namespace Momento.Services.Exceptions
{
    using System;

    public class BadRequestError : Exception
    {
        public BadRequestError(string message):base(message) { }
    }
}
