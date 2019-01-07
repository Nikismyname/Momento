namespace Momento.Services.Implementations.Directory
{
    using System.Linq;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Services.Contracts.Directory;
    using Momento.Models.Directories;
    using System;
    using Momento.Services.Models.DirectoryModels;
    using Momento.Services.Mapping;
    using Momento.Services.Exceptions;
    using Momento.Services.Utilities;

    public class DirectoryService : IDirectoryService
    {
        private readonly MomentoDbContext context;
        public DirectoryService(MomentoDbContext context)
        {
            this.context = context;
        }

        #region Create
        //Verified
        public int Create(int parentDirId, string dirName, string username)
        {
            var parentDir = context.Directories.SingleOrDefault(x => x.Id == parentDirId);

            if (parentDir == null)
            {
                throw new ItemNotFound("The parent directory of the directory you are trying to create does not exist!");
            }

            var user = this.context.Users.SingleOrDefault(x => x.UserName == username);

            if (user == null)
            {
                throw new UserNotFound(username);
            }

            if (parentDir.UserId != user.Id)
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

        public int CreateApi(int parentDirId, string dirName, string username)
        {
            try
            {
                var id = Create(parentDirId, dirName, username);
                return id;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        public DirectoryIndexSingle GetIndexSingle(int? directoryId, string username)
        {
            var userId = context.Users.SingleOrDefault(x => x.UserName == username)?.Id;

            if (userId == null)
            {
                //throw new UserNotFound(username);
                return null;
            }

            DirectoryIndexSingle dir = null;

            IQueryable<DirectoryIndexSingle> baseQuery = context.Directories.Where(x => x.UserId == userId).To<DirectoryIndexSingle>();

            ///if that, find root
            if (directoryId == null || directoryId == 0)
            {
                dir = baseQuery
                    .SingleOrDefault(x => x.ParentDirectoryId == null);

                if (dir == null)
                {
                    //throw new InternalServerError("Could not find root dir for user " + username);
                    return null;
                }
            }
            else
            {
                dir = baseQuery
                    .SingleOrDefault(x => x.Id == directoryId);

                if (dir == null)
                {
                    // new BadRequestError("The directory you are trying to access eather does not exist or is not yours!");
                    return null;
                }
            }

            return dir;
        }

        public DirectoryIndexSingle GetIndexSingleApi(int? directoryId, string username)
        {
            try
            {
                var result = GetIndexSingle(directoryId, username);
                return result;
            }
            catch
            {
                return null;
            }
        }


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

        /// <summary>
        /// Soft delete
        /// Deletes: Videos, ListToDo, notes, comparisons
        /// </summary>
        /// Validated
        /// T
        public void Delete(int id, string username)
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

            if (dirToDelete.UserId != user.Id)
            {
                throw new AccessDenied("The directory you are trying to delete does not belong to you!");
            }

            if(dirToDelete.Name == Constants.RootDorectoryName)
            {
                throw new AccessDenied("Can not delete Root directory!");
            }

            var now = DateTime.UtcNow;
            this.SoftDeleteDirectoryRec(dirToDelete, now);
            context.SaveChanges();
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

        private void SoftDeleteDirectoryRec(Directory dir, DateTime now)
        {
            dir = this.context.Directories
                .Include(x => x.Notes)
                    .ThenInclude(x => x.Lines)
                .Include(x => x.ListsToDo)
                    .ThenInclude(x => x.Items)
                .Include(x=>x.Videos)
                    .ThenInclude(x=>x.Notes)
                .Include(x=>x.Comparisons)
                    .ThenInclude(x=>x.Items)
                .Include(x=>x.Subdirectories)
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

        #region Deprecated
        ///Deprecated
        public DirectoryIndex GetIndex(string username)
        {
            var dirs = context.Directories
                    .Include(x => x.Videos)
                        .ThenInclude(x => x.Notes)
                    .Include(x => x.ListsToDo)
                .Where(x => x.User.UserName == username)
                .ToArray();

            var dir = dirs.SingleOrDefault(x => x.ParentDirectoryId == null);

            var indexDir = Mapper.Instance.Map<DirectoryIndex>(dir);
            return indexDir;
        }

        private void HardDelete(int id)
        {
            CascadeDelete(id);
        }

        private void CascadeDelete(int dirId)
        {
            var dirInfo = context.Directories
                .Select(x => new
                {
                    dir = x,
                    subdirs = x.Subdirectories.Select(y => y.Id).ToArray(),
                    contents = x.Videos.Select(z => z.Notes.Where(y => y.NoteId == null).Select(y => y.Id).ToArray()).ToArray()
                })
                .SingleOrDefault(x => x.dir.Id == dirId);

            foreach (var cnt in dirInfo.contents)
            {
                foreach (int rootDirId in cnt)
                {
                    CascadeDeleteVideoNotes(rootDirId);
                }
            }

            foreach (var subDirId in dirInfo.subdirs)
            {
                CascadeDelete(subDirId);
            }

            context.Directories.Remove(dirInfo.dir);
            context.SaveChanges();
        }

        private void CascadeDeleteVideoNotes(int noteId)
        {
            var noteInfo = context.VideoNotes
                .Select(x => new { note = x, subnotes = x.ChildNotes.Select(y => y.Id).ToArray() })
                .SingleOrDefault(x => x.note.Id == noteId);

            foreach (var subnoteId in noteInfo.subnotes)
            {
                CascadeDeleteVideoNotes(subnoteId);
            }

            context.VideoNotes.Remove(noteInfo.note);
            context.SaveChanges();
        }
        #endregion
    }
}
