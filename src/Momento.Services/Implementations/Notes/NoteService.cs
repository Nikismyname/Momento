namespace Momento.Services.Implementations.Notes
{
    #region Initialization
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Models.Notes;
    using Momento.Models.Users;
    using Momento.Services.Contracts.Notes;
    using Momento.Services.Contracts.Shared;
    using Momento.Services.Contracts.Utilities;
    using Momento.Services.Exceptions;
    using Momento.Services.Models.NoteModels;
    using System;
    using System.Linq;

    ///TestedR1
    public class NoteService : INoteService
    {
        private readonly MomentoDbContext context;
        private readonly IUtilitiesService utilService;
        ///TODO: Add the tracking
        private readonly ITrackableService trackableService;

        public NoteService(
            MomentoDbContext context,
            IUtilitiesService utilService,
            ITrackableService trackableService)
        {
            this.context = context;
            this.utilService = utilService;
            this.trackableService = trackableService;
        }
        #endregion

        #region Create
        ///Tested
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
                throw new AccessDenied("The directory you are trying to add a note to does not beling to you!");
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
        #endregion

        #region GetForEdit
        ///Tested
        public NoteEdit GetForEdit(int id, string username)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var note = context.Notes
                .Include(x => x.Lines)
                .SingleOrDefault(x => x.Id == id);
            if (note == null)
            {
                throw new ItemNotFound("The note you are trying to get does not exist!");
            }

            if (note.UserId != user.Id)
            {
                throw new AccessDenied("The note you are trying to get does not belong to you!");
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
        #endregion

        #region Save
        ///Tested
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

        private void CheckSaveForAuthorization(ref User user, ref Note pageNote, string username, int noteId)
        {
            user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            pageNote = context.Notes.SingleOrDefault(x => x.Id == noteId);
            if (pageNote == null)
            {
                throw new ItemNotFound("The note you are trying to edit does not exist!");
            }

            if (pageNote.UserId != user.Id)
            {
                throw new AccessDenied("The note you are trying to edit does not belong to you!");
            }
        }

        private void UpdateExistingNotes(Note dbNote, NoteEdit editInput)
        {
            //var validIds = dbNote.Lines.Select(x => x.Id);

            var validIds = context.Notes
                .Where(x=>x.Id == dbNote.Id)
                .Select(x=>x.Lines.Select(y=> y.Id).ToArray())
                .SingleOrDefault();

            var existingLinesIds = editInput.Lines.Where(x => x.Id > 0).Select(x => x.Id);

            if (existingLinesIds.Any(x => !validIds.Contains(x)))
            {
                throw new AccessDenied("The Lines you are trying to edit do not belong the Note you are editing!");
            }

            //var dbLinesToUpdate = dbNote.Lines.Where(x => existingLinesIds.Contains(x.Id)).OrderBy(x => x.Id).ToArray();
            var dbLinesToUpdate = this.context.Notes
                .Where(x => x.Id == dbNote.Id)
                .Select(x=>x.Lines
                    .Where(y=> existingLinesIds.Contains(y.Id))
                    .OrderBy(y=>y.Id)
                    .ToArray())
                .SingleOrDefault();

            var pageLines = editInput.Lines.Where(x => x.Id > 0).OrderBy(x => x.Id).ToArray();

            if (dbLinesToUpdate.Length != pageLines.Length) throw new Exception("Lines To update from page does not match lines to update from db!");

            for (int i = 0; i < dbLinesToUpdate.Length; i++)
            {
                var currentDbNote = dbLinesToUpdate[i];
                var pageLine = pageLines[i];

                if (currentDbNote.Id != pageLine.Id) throw new Exception("Line To update from page does not match line to update from db [ID]!");

                this.utilService.UpdatePropertiesReflection(pageLine, currentDbNote, new string[] { "Id" });
            }
        }

        public void DeleteRemovedLines(NoteEdit editInput, Note dbNote)
        {
            var currentDbIds = context.Notes
                .Where(x => x.Id == dbNote.Id)
                .Select(x => x.Lines
                    .Select(y => y.Id)
                    .ToArray())
                .SingleOrDefault();

            var currentPageIds = editInput.Lines.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
            var removedIds = currentDbIds.Where(x => !currentPageIds.Contains(x));

            var codeLinesToRemove = context.Notes
                .Where(x => x.Id == dbNote.Id)
                .Select(x=>x.Lines
                    .Where(y=>removedIds.Contains(y.Id))
                    .ToArray())
                .SingleOrDefault();

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

        #endregion

        #region Delete
        ///Tested
        public Note Delete(int id, string username)
        {
            var userId = this.context.Users.SingleOrDefault(x => x.UserName == username)?.Id;
            if (userId == null)
            {
                throw new UserNotFound(username);
            }

            var note = this.context.Notes
                .Include(x=>x.Lines)
                .SingleOrDefault(x => x.Id == id);

            if (note == null)
            {
                throw new ItemNotFound("The note you are trying to delete does not exist!");
            }

            if (userId != note.UserId)
            {
                throw new AccessDenied("The note you are trying to delete does not belong to you!");
            }

            var now = DateTime.UtcNow;

            foreach (var item in note.Lines)
            {
                item.DeletedOn = now;
                item.IsDeleted = true;
            }

            note.DeletedOn = now;
            note.IsDeleted = true;

            context.SaveChanges();

            return note;
        }

        public bool DeleteApi(int id, string username)
        {
            try
            {
                this.Delete(id, username);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
