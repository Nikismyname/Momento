namespace Momento.Services.Contracts.ListToDo
{
    using Momento.Services.Models.ListToDoModels;

    public interface IListToDoService
    {
        ListToDoUse GetUseModel(int id);
        void Create(ListToDoCreate model);
        void Delete(int id);
        void Save(ListToDoUse model, string username);
    }
}
