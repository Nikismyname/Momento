namespace Momento.Services.Implementations.Directory
{
    using AutoMapper;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Momento.Services.Contracts.Directory;
    using Momento.Models.Directories;
    using System;
    using Models.DirectoryModels;
    using Mapping;
    using Exceptions;
    using Momento.Services.Utilities;

    public class DirectoryService : IDirectoryService
    {
        private readonly MomentoDbContext context;
        public DirectoryService(MomentoDbContext context)
        {
            this.context = context;
        }

        #region Create ///T
        ///Verified
        ///Tested
        public int Create(int parentDirId, string dirName, string username, bool isAdmin = false)
        {
            var user = this.context.Users.SingleOrDefault(x => x.UserName == username);

            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var parentDir = context.Directories.SingleOrDefault(x => x.Id == parentDirId);

            if (parentDir == null)
            {
                throw new ItemNotFound("The parent directory of the directory you are trying to create does not exist!");
            }

            if (parentDir.UserId != user.Id && isAdmin == false)
            {
                throw new AccessDenied("The parent directory does not belong to you!");
            }

            if (string.IsNullOrWhiteSpace(dirName) || dirName.Length == 0 || dirName.ToLower() == "root")
            {
                throw new BadRequestError("The directory name is not valid!");
            }

            var order = 0;
            if (parentDir.Subdirectories.Any())
            {
                order = parentDir.Subdirectories.Select(x => x.Order).Max() + 1;
            }

            var dir = new Directory
            {
                Name = dirName,
                UserId = parentDir.UserId,
                ParentDirectory = parentDir,
                Order = order,
            };

            context.Directories.Add(dir);
            context.SaveChanges();

            return dir.Id;
        }

        public int CreateApi(int parentDirId, string dirName, string username , bool isAdmin = false)
        {
            try
            {
                var id = Create(parentDirId, dirName, username, isAdmin);
                return id;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region GetIndexSingle ///TM
        ///Tested Mostly
        public DirectoryIndexSingle GetIndexSingle(int? directoryId, string username, bool isAdmin = false)
        {
            var userId = context.Users.SingleOrDefault(x => x.UserName == username)?.Id;

            if (userId == null)
            {
                throw new UserNotFound(username);
            }

            DirectoryIndexSingle dir = null;

            ///if that, find root
            if (directoryId == null || directoryId == 0)
            {
                dir = context.Directories
                    .Where(x => x.UserId == userId)
                    .To<DirectoryIndexSingle>()
                    .FirstOrDefault(x => x.Name == Constants.RootDorectoryName);

                if (dir == null)
                {
                    throw new InternalServerError("Could not find root dir for user " + username);
                }
            }
            else
            {
                var dbDir = this.context.Directories
                    .SingleOrDefault(x => x.Id == directoryId);

                if (dbDir == null)
                {
                    throw new BadRequestError("The directory you are trying to access eather does not exist or is not yours!");
                }

                if (dbDir.UserId != userId && isAdmin == false)
                {
                    throw new BadRequestError("The directory you are trying to access eather does not exist or is not yours!");
                }

                dir = this.context.Directories
                    .To<DirectoryIndexSingle>()
                    .SingleOrDefault(x => x.Id == directoryId);
            }

            return dir;
        }

        public DirectoryIndexSingle GetIndexSingleApi(int? directoryId, string username, bool isAdmin = false)
        {
            try
            {
                var result = GetIndexSingle(directoryId, username, isAdmin);
                return result;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region CreaterRoot ///Tested Needs Some validation maybe, even though it is internal function 
        public void CreateRoot(string username)
        {
            var userId = context.Users.SingleOrDefault(x => x.UserName == username).Id;
            var dir = new Directory
            {
                Name = Constants.RootDorectoryName,
                UserId = userId,
            };

            context.Directories.Add(dir);
            context.SaveChanges();
        }
        #endregion

        #region Delete ///T
        /// <summary>
        /// Soft delete
        /// Deletes: Videos, ListToDo, notes, comparisons
        /// </summary>
        /// Validated
        /// Tested
        public Directory Delete(int id, string username, bool isAdmin = false)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var dirToDelete = context.Directories.SingleOrDefault(x => x.Id == id);

            if (dirToDelete == null)
            {
                throw new ItemNotFound("The directory you are trying to delete does not exist!");
            }

            if (dirToDelete.UserId != user.Id && isAdmin == false)
            {
                throw new AccessDenied("The directory you are trying to delete does not belong to you!");
            }

            if (dirToDelete.Name == Constants.RootDorectoryName)
            {
                throw new AccessDenied("Can not delete Root directory!");
            }

            var now = DateTime.UtcNow;
            this.SoftDeleteDirectoryRec(dirToDelete, now);
            context.SaveChanges();

            return dirToDelete;
        }

        public bool DeleteApi(int id, string username, bool isAdmin = false)
        {
            try
            {
                this.Delete(id, username, isAdmin);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SoftDeleteDirectoryRec(Directory dir, DateTime now)
        {
            dir = this.context.Directories
                .Include(x => x.Notes)
                    .ThenInclude(x => x.Lines)
                .Include(x => x.ListsToDo)
                    .ThenInclude(x => x.Items)
                .Include(x => x.Videos)
                    .ThenInclude(x => x.Notes)
                .Include(x => x.Comparisons)
                    .ThenInclude(x => x.Items)
                .Include(x => x.Subdirectories)
                .Single(x => x.Id == dir.Id);

            ///Videos
            var videos = dir.Videos;
            foreach (var vid in videos)
            {
                var videoNotes = vid.Notes;
                foreach (var note in videoNotes)
                {
                    note.DeletedOn = now;
                    note.IsDeleted = true;
                }

                vid.DeletedOn = now;
                vid.IsDeleted = true;
            }
            ///...

            ///ListsToDo
            var listsToDo = dir.ListsToDo;
            foreach (var todo in listsToDo)
            {
                var items = todo.Items;
                foreach (var item in items)
                {
                    item.DeletedOn = now;
                    item.IsDeleted = true;
                }

                todo.DeletedOn = now;
                todo.IsDeleted = true;
            }
            ///...

            ///Comparisons
            var comparisons = dir.Comparisons;
            foreach (var comp in comparisons)
            {
                var items = comp.Items;
                foreach (var item in items)
                {
                    item.DeletedOn = now;
                    item.IsDeleted = true;
                }

                comp.DeletedOn = now;
                comp.IsDeleted = true;
            }
            ///...

            ///Notes
            var notes = dir.Notes;
            foreach (var note in notes)
            {
                var lines = note.Lines;
                foreach (var line in lines)
                {
                    line.DeletedOn = now;
                    line.IsDeleted = true;
                }

                note.DeletedOn = now;
                note.IsDeleted = true;
            }
            ///...


            ///SubDirectories Recursion here
            var subdirs = dir.Subdirectories;
            foreach (var subdir in subdirs)
            {
                SoftDeleteDirectoryRec(subdir, now);
            }
            ///...

            dir.DeletedOn = now;
            dir.IsDeleted = true;
        }
        #endregion

        #region Deprecated
        ///Deprecated
        //public DirectoryIndex GetIndex(string username)
        //{
        //    var dirs = context.Directories
        //            .Include(x => x.Videos)
        //                .ThenInclude(x => x.Notes)
        //            .Include(x => x.ListsToDo)
        //        .Where(x => x.User.UserName == username)
        //        .ToArray();

        //    var dir = dirs.SingleOrDefault(x => x.ParentDirectoryId == null);

        //    var indexDir = Mapper.Instance.Map<DirectoryIndex>(dir);
        //    return indexDir;
        //}

        //private void HardDelete(int id)
        //{
        //    CascadeDelete(id);
        //}

        //private void CascadeDelete(int dirId)
        //{
        //    var dirInfo = context.Directories
        //        .Select(x => new
        //        {
        //            dir = x,
        //            subdirs = x.Subdirectories.Select(y => y.Id).ToArray(),
        //            contents = x.Videos.Select(z => z.Notes.Where(y => y.NoteId == null).Select(y => y.Id).ToArray()).ToArray()
        //        })
        //        .SingleOrDefault(x => x.dir.Id == dirId);

        //    foreach (var cnt in dirInfo.contents)
        //    {
        //        foreach (int rootDirId in cnt)
        //        {
        //            CascadeDeleteVideoNotes(rootDirId);
        //        }
        //    }

        //    foreach (var subDirId in dirInfo.subdirs)
        //    {
        //        CascadeDelete(subDirId);
        //    }

        //    context.Directories.Remove(dirInfo.dir);
        //    context.SaveChanges();
        //}

        //private void CascadeDeleteVideoNotes(int noteId)
        //{
        //    var noteInfo = context.VideoNotes
        //        .Select(x => new { note = x, subnotes = x.ChildNotes.Select(y => y.Id).ToArray() })
        //        .SingleOrDefault(x => x.note.Id == noteId);

        //    foreach (var subnoteId in noteInfo.subnotes)
        //    {
        //        CascadeDeleteVideoNotes(subnoteId);
        //    }

        //    context.VideoNotes.Remove(noteInfo.note);
        //    context.SaveChanges();
        //}
        #endregion
    }
}
