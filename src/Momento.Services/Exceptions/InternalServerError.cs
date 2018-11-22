namespace Momento.Services.Exceptions
{
    using System;

    public class InternalServerError:Exception
    {
        public InternalServerError(string message) : base(message) { }
    }
}
