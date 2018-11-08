namespace Momento.Data.Models.Contracts
{
    using System;

    public interface IChangeTrackable
    {
        DateTime? LastModifiedOn { get; set; }
    }
}
