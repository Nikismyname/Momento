namespace Momento.Models.Contracts
{
    public interface IOrderable<T>
    {
        T Id { get; set; }
        int Order { get; set; }
    }
}
