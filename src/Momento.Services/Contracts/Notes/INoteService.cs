namespace Momento.Services.Contracts.Notes
{
    using Momento.Models.Notes;
    using Momento.Services.Models.NoteModels;

    public interface INoteService
    {
        Note Create(NoteCreate note, string username);

        bool CreateApi(NoteCreate note, string username);

        NoteEdit GetForEdit(int id, string username);

        NoteEdit GetForEditApi(int id, string username);

        Note Save(NoteEdit model, string username);

        bool SaveApi(NoteEdit model, string username);

        void Delete(int id, string username);

        bool DeleteApi(int id, string username);
    }
}
