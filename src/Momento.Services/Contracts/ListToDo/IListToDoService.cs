namespace Momento.Services.Contracts.ListToDo
{
    using Momento.Services.Models.ListToDoModels;

    public interface IListToDoService
    {
        ListToDoUse GetUseModel(int id, string username, bool isAdmin = false);

        Momento.Models.ListsToDo.ListToDo Create(
            ListToDoCreate model, string username,bool isAdmin = false);

        Momento.Models.ListsToDo.ListToDo Delete(int id, string username, bool isAdmin = false);

        bool DeleteApi(int id, string username, bool isAdmin = false);

        void Save(ListToDoUse model, string username, bool isAdmin = false);
    }
}
