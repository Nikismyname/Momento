namespace Momento.Data.Models
{
    using Momento.Data.Models.Contracts;
    using System;

    public abstract class BaseModel<T> : IBaseModel<T>
    {
        public T Id { get; set; }
    }
}
