namespace Momento.Models.Contracts
{
    using System;

    public interface ITrackable
    {
        DateTime? CreatedOn { get; set; }
        DateTime? LastModifiedOn { get; set; }
        DateTime? LastViewdOn { get; set; }
        int TimesModified { get; set; }
        int TimesViewd { get; set; }
    }
}
