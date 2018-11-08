namespace Momento.Data.Models.Contracts
{
    public interface IBaseModel<T> 
    {
        T Id { get; set; }
    }
}
