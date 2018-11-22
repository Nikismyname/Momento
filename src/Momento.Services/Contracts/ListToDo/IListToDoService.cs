namespace Momento.Services.Contracts.ListToDo
{
    using Momento.Services.Models.ListToDoModels;

    public interface IListToDoService
    {
        ListToDoUse GetUseModel(int id, string username);

        void Create(ListToDoCreate model, string username);

        void Delete(int id, string username);

        void Save(ListToDoUse model, string username);
    }
}
