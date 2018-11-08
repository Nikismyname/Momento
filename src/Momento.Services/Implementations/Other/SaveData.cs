namespace Momento.Services.Implementations.Other
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using Momento.Data;
    using Momento.Services.Contracts.Other;
    using System.Linq;
    using Newtonsoft.Json;
    using Momento.Data.Models.Videos;
    using Momento.Data.Models.Directories;


    public class SaveData : ISaveData
    {
        private readonly MomentoDbContext context;

        public SaveData(MomentoDbContext context)
        {
            this.context = context;
        }

        public string GetDirectoryName(int directoryId)
        {
            var dirInfo = context.Directories
                .Select(x => new { id = x.Id, name = x.Name })
                .SingleOrDefault(x => x.id == directoryId);
            return dirInfo.name;
        }

        //TODO: chack if the user has access to those directories
        public string GetDirectoryData(int directoryId)
        {
            var dir = LoadAllData(directoryId);

            if (dir == null)
            {
                return "There is no such directory";
            }

            var result = JsonConvert.SerializeObject(dir,Formatting.Indented);
            result = "Version 1" + Environment.NewLine + result;
            return result;
        }

        private Directory LoadAllData(int id)
        {
            var dirInfo = context.Directories
                .Include(x => x.Videos)
                    .ThenInclude(x=>x.Notes)
                .Select(x=> new {dir =x, children = x.Subdirectories.Select(y=>y.Id)})
                .SingleOrDefault(x=>x.dir.Id==id);

            var dir = dirInfo.dir;

            foreach (var cnt in dir.Videos)
            {
                cnt.Directiry = null;

                foreach (var note in cnt.Notes)
                {
                    note.Note = null;
                    note.Video = null;
                }

                ///removing the notes who are not ROOT
                ///since it becomes a duplication otherwise
                cnt.Notes = cnt.Notes.Where(x => x.NoteId == null).ToArray();
            }

            dir.ParentDirectory = null;

            foreach (var subdirId in dirInfo.children)
            {
                dir.Subdirectories.Add(LoadAllData(subdirId));
            }

            return dir;
        }

        public void UploadData(string json, int parentDir)
        {
            json = string.Join(Environment.NewLine, json.Split(Environment.NewLine).Skip(1));

            var directory = (Directory)JsonConvert.DeserializeObject(json, typeof (Directory));
            FixIds(directory);

            directory.ParentDirectory = null;
            directory.ParentDirectoryId = parentDir;

            context.Directories.Add(directory);
            context.SaveChanges();
        }

        private void FixIds (Directory dir)
        {
            dir.Id = default(int);
            dir.ParentDirectory = null;
            dir.ParentDirectoryId = default(int);

            foreach (var cnt in dir.Videos)
            {
                cnt.Id = default(int);
                cnt.Directiry = dir;
                cnt.DirectoryId = default(int);

                FixIdNotes(cnt.Notes.ToArray(),cnt);
            }

            foreach (var subdir in dir.Subdirectories)
            {
                FixIds(subdir);
            }
        }

        private void FixIdNotes(VideoNote[] notes,Video cnt)
        {
            foreach (var note in notes)
            {
                note.Id = default(int);
                note.NoteId = null;
                note.Video = cnt;
                note.Note = null;

                FixIdNotes(note.ChildNotes.ToArray(),cnt);
            }
        }
    }
}
