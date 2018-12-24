namespace Momento.Services.Implementations.Notes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Models.Notes;
    using Momento.Models.Users;
    using Momento.Services.Contracts.Notes;
    using Momento.Services.Contracts.Utilities;
    using Momento.Services.Exceptions;
    using Momento.Services.Models.NoteModels;
    using System;
    using System.Linq;

    public class NoteService : INoteService
    {
        private readonly MomentoDbContext context;
        private readonly IUtilitiesService utilService;

        public NoteService(MomentoDbContext context, IUtilitiesService utilService)
        {
            this.context = context;
            this.utilService = utilService;
        }

        public Note Create(NoteCreate note, string username)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var directory = context.Directories.SingleOrDefault(x => x.Id == note.DirectoryId);
            if (directory == null)
            {
                throw new ItemNotFound("The directory you are trying to add a note to does not exist!");
            }

            if (user.Id != directory.UserId)
            {
                throw new AccessDenied("The directory you are trying to add a note to des not exist!");
            }

            var order = directory.Notes.Count == 0 ? 0 : directory.Notes.Select(x => x.Order).Max() + 1;

            var resultNote = new Note
            {
                Name = note.Name,
                Description = note.Description,
                Order = order,
                DirectoryId = note.DirectoryId,
                UserId = user.Id,
            };

            context.Notes.Add(resultNote);
            context.SaveChanges();

            return resultNote;
        }

        public bool CreateApi(NoteCreate note, string username)
        {
            try
            {
                Create(note, username);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public NoteEdit GetForEdit(int id, string username)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var note = context.Notes
                .Include(x=>x.Lines)
                .SingleOrDefault(x => x.Id == id);
            if (note == null)
            {
                throw new ItemNotFound("The note you are trying to get does not exist!");
            }

            if(note.UserId != user.Id)
            {
                throw new UserNotFound("The note you are trying to get does not belong to you!");
            }

            var resultNote = Mapper.Instance.Map<NoteEdit>(note);
            return resultNote;
        }

        public NoteEdit GetForEditApi(int id, string username)
        {
            try
            {
                return this.GetForEdit(id, username);
            }
            catch
            {
                return null;
            }
        }

        public Note Save(NoteEdit editInput, string username)
        {
            User user = null;
            Note dbNote = null;
            this.CheckSaveForAuthorization(ref user, ref dbNote, username, editInput.Id);
            this.utilService.UpdatePropertiesReflection(editInput, dbNote, new string[] { "Lines", "Id", "Name", "Description" });
            this.UpdateExistingNotes(dbNote, editInput);
            this.DeleteRemovedLines(editInput, dbNote);
            this.SaveNewLines(editInput);
            context.SaveChanges();

            return dbNote;
        }

        private void CheckSaveForAuthorization(ref User user, ref Note pageNote, string username, int noteId)
        {
            user = context.Users.SingleOrDefault(x => x.UserName == username);
            if(user == null)
            {
                throw new UserNotFound(username);
            }

            pageNote = context.Notes.SingleOrDefault(x => x.Id == noteId);
            if(pageNote == null)
            {
                throw new ItemNotFound("The note you are trying to edit does not exist!");
            }

            if(pageNote.UserId != user.Id)
            {
                throw new AccessDenied("The note you are trying to edit does not belong to you!");
            }
        }

        private void UpdateExistingNotes(Note note, NoteEdit editInput)
        {
            var validIds = note.Lines.Select(x => x.Id);
            var existingLinesIds = editInput.Lines.Where(x => x.Id > 0).Select(x=>x.Id);

            if (existingLinesIds.Any(x => !validIds.Contains(x)))
            {
                throw new AccessDenied("The Lines you are trying to edit do not belong the Note you are editing!");
            }

            var dbLinesToUpdate = note.Lines.Where(x=> existingLinesIds.Contains(x.Id)).OrderBy(x=>x.Id).ToArray();
            var pageLines = editInput.Lines.Where(x => x.Id > 0).OrderBy(x => x.Id).ToArray();

            for (int i = 0; i < dbLinesToUpdate.Length; i++)
            {
                var dbNote = dbLinesToUpdate[i];
                var pageLine = pageLines[i];

                if(dbNote.Id != pageLine.Id)
                {
                    throw new Exception("You done goofed");
                }

                this.utilService.UpdatePropertiesReflection(pageLine,dbNote,new string[] {"Id" });
            }
        }

        public void DeleteRemovedLines(NoteEdit editInput, Note dbNote)
        {
            var currentDbIds = dbNote.Lines.Select(x => x.Id).ToArray();
            var currentPageIds = editInput.Lines.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
            var removedIds = currentDbIds.Where(x => !currentPageIds.Contains(x));
            var codeLinesToRemove = dbNote.Lines.Where(x => removedIds.Contains(x.Id));
            var now = DateTime.UtcNow;
            foreach (var item in codeLinesToRemove)
            {
                item.DeletedOn = now;
                item.IsDeleted = true;
            }
        }

        private void SaveNewLines(NoteEdit editInput)
        {
            var newPageLines = editInput.Lines.Where(x => x.Id == 0).ToArray();
            var newDbNotes = newPageLines.Select(x => Mapper.Instance.Map<CodeLine>(x)).ToArray();
            foreach (var item in newDbNotes)
            {
                item.NoteId = editInput.Id;
            }
            context.CodeLines.AddRange(newDbNotes);
        }

        public bool SaveApi(NoteEdit model, string username)
        {
            try
            {
                this.Save(model, username);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
