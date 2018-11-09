namespace Momento.Services.Contracts.ListRemind
{
    using Momento.Services.Models.ListRemind;
    using System.Collections.Generic;

    public interface IListRemindService
    {
        List<ListRemindIndex> GetIndex(string userId);

        void Create(string username, string name, List<ListRemindItemCreate> listItems);

        ListRemindCreate GetEdit(int id);

        void Edit(int listId, string name, List<ListRemindItemCreate> listItems);

        void Delete(int id);

        string GetName(int id);
    }
}
