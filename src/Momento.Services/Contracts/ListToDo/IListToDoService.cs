namespace Momento.Services.Contracts.ListToDo
{
    using Momento.Services.Models.ListToDoModels;

    public interface IListToDoService
    {
        ListToDoUse GetUseModel(int id, string username);

        Momento.Models.ListsToDo.ListToDo Create(ListToDoCreate model, string username);

        Momento.Models.ListsToDo.ListToDo Delete(int id, string username);

        bool DeleteApi(int id, string username);

        void Save(ListToDoUse model, string username);
    }
}
