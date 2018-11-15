namespace Momento.Models
{
    using Momento.Models.Contracts;
    using System;

    public abstract class BaseModel<T> : IBaseModel<T>
    {
        public T Id { get; set; }
    }
}
