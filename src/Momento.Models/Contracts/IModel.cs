namespace Momento.Models.Contracts
{
    public interface IBaseModel<T> 
    {
        T Id { get; set; }
    }
}
