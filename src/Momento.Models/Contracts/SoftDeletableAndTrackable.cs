namespace Momento.Models.Contracts
{
    using System;

    public abstract class SoftDeletableAndTrackable :ISoftDeletableAndTrackable
    {
        protected SoftDeletableAndTrackable()
        {
            this.IsDeleted = false;
            var now = DateTime.UtcNow;
            this.CreatedOn = now;
            this.LastModifiedOn = now;
            this.LastViewdOn = now;
            this.DeletedOn = null;
            this.TimesModified = 1;
            this.TimesViewd = 1;
        }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public DateTime? LastViewdOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int TimesModified { get; set; }

        public int TimesViewd { get; set; }
    }
}
