namespace Momento.Services.Contracts.ListRemind
{
    using Momento.Services.Models.List;
    using System.Collections.Generic;

    public interface IListRemindService
    {
        List<ListIndex> GetIndex(string userId);

        void Create(string username, string name, List<ListItemCreate> listItems);

        ListCreate GetEdit(int id);

        void Edit(int listId, string name, List<ListItemCreate> listItems);

        void Delete(int id);

        string GetName(int id);
    }
}
