namespace Momento.Services.Contracts.Notes
{
    using Momento.Models.Notes;
    using Momento.Services.Models.NoteModels;

    public interface INoteService
    {
        Note Create(NoteCreate note, string username, bool isAdmin = false);

        bool CreateApi(NoteCreate note, string username, bool isAdmin = false);

        NoteEdit GetForEdit(int id, string username, bool isAdmin = false);

        NoteEdit GetForEditApi(int id, string username, bool isAdmin = false);

        Note Save(NoteEdit model, string username, bool isAdmin = false);

        bool SaveApi(NoteEdit model, string username, bool isAdmin = false);

        Note Delete(int id, string username, bool isAdmin = false);

        bool DeleteApi(int id, string username, bool isAdmin = false);
    }
}
