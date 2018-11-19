namespace Momento.Models.Contracts
{
    using System;

    public abstract class SoftDeletable : ISoftDeletable
    {
        public SoftDeletable()
        {
            this.IsDeleted = false;
            this.DeletedOn = null;
        }
        
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
