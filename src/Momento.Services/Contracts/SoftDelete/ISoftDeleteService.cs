namespace Momento.Services.Contracts.SoftDelete
{
    using Momento.Models.Contracts;

    public interface ISoftDeleteService
    {
        void SoftCascadeDelete(ISoftDeletable model);
    }
}
