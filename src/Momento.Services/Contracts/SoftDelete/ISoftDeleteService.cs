namespace Momento.Services.Contracts.SoftDelete
{
    using Momento.Data.Models.Contracts;

    public interface ISoftDeleteService
    {
        void SoftCascadeDelete(ISoftDeletable model);
    }
}
