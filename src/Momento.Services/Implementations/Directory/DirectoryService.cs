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
    using Momento.Services.Exceptions;
    using Momento.Services.Mapping;
    using MomentoServices.Models.DirectoryModels;

    public class DirectoryService : IDirectoryService
    {
        private readonly MomentoDbContext context;
        private readonly IMapper mapper;

        public DirectoryService(MomentoDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Create(int parentDirId, string name, string userName)
        {
            var parentDir = context.Directories.SingleOrDefault(x => x.Id == parentDirId);
            var order = 0;
            if (parentDir.Subdirectories.Any())
            {
                order = parentDir.Subdirectories.Select(x => x.Order).Max() + 1;
            }

            var dir = new Directory
            {
                Name = name,
                UserId = parentDir.UserId,
                ParentDirectory = parentDir,
                Order = order,
            };

            context.Directories.Add(dir);
            context.SaveChanges();
        }

        public DirectoryIndex GetIndex(string username)
        {
            ///TODO: do the smarter IQueriable mapping might save some time 
            var dirs = context.Directories
                    .Include(x => x.Videos)
                        .ThenInclude(x => x.Notes)
                    .Include(x => x.ListsToDo)
                .Where(x => x.User.UserName == username)
                .ToArray();

            var dir = dirs.SingleOrDefault(x => x.ParentDirectoryId == null);

            var indexDir = mapper.Map<DirectoryIndex>(dir);
            return indexDir;
        }

        public DirectoryImdexSingle GetIndexSingle(int? directoryId, string username)
        {
            var userId = context.Users.SingleOrDefault(x => x.UserName == username)?.Id;

            if (userId == null)
            {
                //throw new UserNotFound(username);
                return null;
            }

            DirectoryImdexSingle dir = null;

            IQueryable<DirectoryImdexSingle> baseQuery = context.Directories.Where(x=>x.UserId == userId).To<DirectoryImdexSingle>();

            ///if that find root
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

        public void CreateRoot(string username)
        {
            var userId = context.Users.SingleOrDefault(x => x.UserName == username).Id;
            var dir = new Directory
            {
                Name = "Root",
                UserId = userId,
            };

            context.Directories.Add(dir);
            context.SaveChanges();
        }

        /// <summary>
        /// Soft delete
        /// Deletes: Videos, ListToDo
        /// </summary>
        public void Delete(int id)
        {
            var dirToDelete = context.Directories.SingleOrDefault(x => x.Id == id);
            var now = DateTime.UtcNow;
            this.SoftDeleteDirectory(dirToDelete, now);
            context.SaveChanges();
        }

        private void SoftDeleteDirectory(Directory dir, DateTime now)
        {
            var videos = dir.Videos;
            foreach (var vid in videos)
            {
                var notes = vid.Notes;
                foreach (var note in notes)
                {
                    note.DeletedOn = now;
                    note.IsDeleted = true;
                }

                vid.DeletedOn = now;
                vid.IsDeleted = true;
            }

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

            var subdirs = dir.Subdirectories;
            foreach (var subdir in subdirs)
            {
                SoftDeleteDirectory(subdir, now);
            }

            dir.DeletedOn = now;
            dir.IsDeleted = true;
        }

        #region Hard Delete

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
