namespace Momento.Models.Contracts
{
    using System;

    public interface IChangeTrackable
    {
        DateTime? LastModifiedOn { get; set; }
    }
}
