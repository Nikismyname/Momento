namespace Momento.Data.Models.Contracts
{
    using System;

    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
