namespace Momento.Services.Implementations.Video
{
    #region Initialization
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Models.Enums;
    using Momento.Models.Videos;
    using Momento.Services.Contracts.Shared;
    using Momento.Services.Contracts.Video;
    using Momento.Services.Exceptions;
    using Momento.Services.Models.VideoModels;

    public class VideoService : IVideoService
    {
        private readonly MomentoDbContext context;
        private readonly ITrackableService trackableService;

        public VideoService(
            MomentoDbContext context,
            ITrackableService trackableService)
        {
            this.context = context;
            this.trackableService = trackableService;
        }
        #endregion

        #region Get
        ///This is where you can press buttons to move around the video.
        ///Tested basics
        public VideoView GetView(int videoId, string username)
        {
            var user = this.context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var video = context.Videos
            .Include(x => x.Notes)
            .SingleOrDefault(x => x.Id == videoId);

            if (video == null)
            {
                throw new ItemNotFound("Video you are trying to view does not exist in the database!");
            }

            if (video.UserId != user.Id)
            {
                throw new AccessDenied("The video you are trying to view does not belong to you");
            }

            trackableService.RegisterViewing(video, DateTime.UtcNow, true);

            var contentView = Mapper.Instance.Map<VideoView>(video);

            return contentView;
        }

        /// <summary>
        /// Returns null if there is a problem
        /// </summary>
        public VideoView GetViewApi(int videoId, string username)
        {
            try
            {
                var result = this.GetView(videoId, username);
                return result;
            }
            catch
            {
                return null;
            }
        }


        /// Verified that the video belongs to the user
        /// Registered viewing here
        /// <summary>
        /// How the noteCreate items are made:
        /// 1) Create a VideoNoteCreate array the same length as the array with dbNotes
        /// 2) Go through the dbNotes and copy all the non-relational data to the pageNotes 
        ///     -assign inPageIds to i 
        ///     -calculate level
        ///     -In a Dictionary (map) map the dbNote.Id to i
        /// 3) Go through the Notes in pairs where one is the DbNote and the other is the page version 
        ///     -assing InPageParentId for the pageMpte to be what is mapped to the dbNote parent note Id
        ///         if there is a parent note.
        /// </summary>
        /// Tested
        public VideoCreate GetVideoForEdit(int videoId, string username)
        {
            var video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == videoId);

            if (video == null)
            {
                throw new ItemNotFound("The video you are trying to edit does not exist!");
            }

            var userId = context.Users.SingleOrDefault(x => x.UserName == username)?.Id;

            if (userId == null)
            {
                throw new UserNotFound(username);
            }

            ///TODO: Videos should have users, remove when they do.
            if (video.UserId != null)
            {
                if (video.UserId != userId)
                {
                    throw new AccessDenied("You can note edit video that does not belong to you!");
                }
            }

            trackableService.RegisterViewing(video, DateTime.UtcNow, true);

            ///Reordering the notes so that when there are deletions the numbers are sequential
            var dbNotes = video.Notes.OrderBy(x => x.Order).ToArray();
            for (int i = 0; i < dbNotes.Length; i++)
            {
                dbNotes[i].Order = i;
            }
            context.SaveChanges();

            var map = new Dictionary<int, int>();
            var pageNotes = new VideoNoteCreate[dbNotes.Length];

            for (int i = 0; i < dbNotes.Length; i++)
            {
                var dbNote = dbNotes[i];
                ///The inPageId is mapped from the order
                pageNotes[i] = Mapper.Instance.Map<VideoNoteCreate>(dbNote);
                pageNotes[i].Level = Level(dbNote);
                map.Add(dbNote.Id, i);
            }

            for (int i = 0; i < dbNotes.Length; i++)
            {
                var dbNote = dbNotes[i];
                var pageNote = pageNotes[i];

                if (dbNote.NoteId != null)
                {
                    pageNote.InPageParentId = map[(int)dbNote.NoteId];
                }
            }

            var result = new VideoCreate
            {
                Id = video.Id,
                Description = video.Description,
                Url = video.Url,
                Name = video.Name,
                DirectoryId = video.DirectoryId,
                Order = video.Order,
                SeekTo = video.SeekTo,
                Notes = pageNotes.ToList(),
            };

            return result;
        }

        public VideoCreate GetVideoForEditApi(int videoId, string username)
        {
            try
            {
                var result = this.GetVideoForEdit(videoId, username);
                return result;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Create
        ///Tested///Not any more///Tested again
        public Video Create(VideoInitialCreate videoCreate, string username)
        {
            var dirId = videoCreate.DirectoryId;

            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            var directory = context.Directories.SingleOrDefault(x => x.Id == dirId);
            if (directory == null)
            {
                throw new ItemNotFound("The Directory you selected for creating the new video notes in, does not exist!");
            }

            if (user.Id != directory.UserId)
            {
                throw new AccessDenied("The directory you are trying to create a video on does note belong to you!");
            }

            var ordersInfo = context.Directories
                .Select(x => new { id = x.Id, orders = x.Videos.Select(y => y.Order).ToArray() })
                .SingleOrDefault(x => x.id == dirId);
            var order = ordersInfo.orders.Length == 0 ? 0 : ordersInfo.orders.Max() + 1;

            var video = new Video
            {
                DirectoryId = dirId,
                Order = order,
                UserId = user.Id,
                Name = videoCreate.Name,
                Description = videoCreate.Description,
                Url = videoCreate.Url,
                SeekTo = 0,
            };

            context.Videos.Add(video);
            context.SaveChanges();

            return video;
        }

        public bool CreateApi(VideoInitialCreate videoCreate, string username)
        {
            try
            {
                this.Create(videoCreate, username);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Delete
        ///Soft Delete
        ///Tested
        public void Delete(int id, string username, DateTime now)
        {
            var video = context.Videos.SingleOrDefault(x => x.Id == id);
            if (video == null)
            {
                throw new ItemNotFound("The video you are trying to delete does not exist!");
            }

            var user = context.Users.SingleOrDefault(x => x.UserName == username);
            if (user == null)
            {
                throw new UserNotFound(username);
            }

            if (video.UserId != null && video.UserId != user.Id)
            {
                throw new AccessDenied("The video you are trying to delete does not belong to you!");
            }

            foreach (var note in video.Notes)
            {
                note.DeletedOn = now;
                note.IsDeleted = true;
            }

            video.DeletedOn = now;
            video.IsDeleted = true;

            context.SaveChanges();
        }

        public bool DeleteApi(int id, string username)
        {
            var now = DateTime.UtcNow;
            try
            {
                this.Delete(id, username, now);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region PartialSave
        ///Code Map: if you send back a jagged array with first element:
        ///0 - everything is ok
        ///1 - Error
        ///Tested
        public int[][] PartialSave(int videoId, string userName, int? seekTo,
            string name, string desctiption, string[][] changes,
            VideoNoteCreate[] newNotes, bool finalSave)
        {
            this.ValidateSaveAndRegisterModification(videoId, changes, userName, finalSave);
            ///Appy changes to the video fields
            this.PartialSaveVideoFields(videoId, name, desctiption, seekTo);
            ///Appy changes to the existing Notes
            this.PartialSaveChangesToExistingNote(changes);
            ///Create the new notes and return their IDs
            var newNoteDbId = this.PartialSaveNewNotes(newNotes, videoId);
            return newNoteDbId;
        }

        /// I am not doing autosaves yet so I only need to know if save was successful 
        public bool PartialSaveApi(int videoId, string userName, int? seekTo,
             string name, string desctiption, string[][] changes,
             VideoNoteCreate[] newNotes, bool finalSave)
        {
            try
            {
                var result = this.PartialSave(videoId, userName, seekTo, 
                    name, desctiption, changes, newNotes, finalSave);

                return true;
            }
            catch
            {
                return false;
            }
        }

        ///Tested exept the integration with the trackable service
        private void ValidateSaveAndRegisterModification(int videoId, string[][] changes, string username, bool finalSave)
        {
            ///chech if video exists 
            var video = context.Videos.SingleOrDefault(x => x.Id == videoId);
            if (video == null)
            {
                throw new ItemNotFound("The video you are working on does not exists in the database");
            }

            var user = context.Users.
                Include(x => x.Directories)
                .SingleOrDefault(x => x.UserName == username);

            if (user == null)
            {
                throw new UserNotFound(username);
            }

            ///check if the video belongs to the user 
            var userVideoIds = context.Users
                .Select(x => new
                {
                    username = x.UserName,
                    videoIds = x.Directories.SelectMany(y => y.Videos.Select(z => z.Id)).ToArray()
                })
                .SingleOrDefault(x => x.username == username)
                .videoIds;

            if (!userVideoIds.Contains(videoId))
            {
                throw new AccessDenied("The video you are trying to modify does not belong to you!");
            }
            ///check if all the notes being changed belong the video they are coming for;
            var videoNotesIds = context
                .Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == videoId)
                .Notes.Select(x => x.Id).ToArray();
            var sentVideoNoteIds = changes.Select(x => int.Parse(x[0])).ToArray();

            if (sentVideoNoteIds.Any(x => !videoNotesIds.Contains(x)))
            {
                throw new AccessDenied("The video notes you are trying to modify does not belong the the current video");
            }

            if (finalSave)
            {
                ///TODO: see if there is an error because the entity is modified 
                ///in another service
                trackableService.RegisterModification(video, DateTime.UtcNow, false);
            }
        }

        ///Tested
        private void PartialSaveVideoFields(int videoId, string name, string description, int? seekTo)
        {
            var video = context.Videos.SingleOrDefault(x => x.Id == videoId);
            
            if (name != null)
            {
                video.Name = name;
            }
            if (description != null)
            {
                video.Description = description;
            }
            if (seekTo != null)
            {
                video.SeekTo = seekTo;
            }
        }

        ///Tested
        private bool PartialSaveChangesToExistingNote(string[][] changes)
        {
            if (changes.Length == 0)
            {
                return true;
            }

            var videoIds = changes.Select(x => int.Parse(x[0])).ToArray();
            var notesToChange = context.VideoNotes.Where(x => videoIds.Contains(x.Id)).ToArray();

            ///Registering modification on the notes
            trackableService.RegisterModificationMany(notesToChange, DateTime.UtcNow, false);

            foreach (var change in changes)
            {
                var noteId = int.Parse(change[0]);
                var prop = change[1];
                var newVal = change[2];
                VideoNote currentNote = notesToChange.SingleOrDefault(x => x.Id == noteId);

                switch (prop.ToUpper())
                {
                    case "DELETED":
                        currentNote.IsDeleted = true;
                        currentNote.DeletedOn = DateTime.UtcNow;
                        break;
                    case "CONTENT":
                        currentNote.Content = newVal;
                        break;
                    case "FORMATTING":
                        currentNote.Formatting = (Formatting)int.Parse(newVal);
                        break;
                    case "SEEKTO":
                        currentNote.SeekTo = int.Parse(newVal);
                        break;
                    case "TYPE":
                        currentNote.Type = (VideoNoteType)int.Parse(newVal); ///TODO: unit test this
                        break;
                    case "BORDERCOLOR":
                        currentNote.BorderColor = newVal;
                        break;
                    case "BORDERTHICKNESS":
                        currentNote.BorderThickness = int.Parse(newVal);
                        break;
                }
            }
            return true;
        }

        ///Tested
        private int[][] PartialSaveNewNotes(VideoNoteCreate[] newPageNotes, int videoId)
        {
            ///removing items which are created and deleted in the same save window
            newPageNotes = newPageNotes.Where(x => x.Deleted == false).ToArray();

            var existingParentNotesIds = newPageNotes
                .Select(x => x.ParentDbId)
                .Where(x => x > 0)
                .ToArray();

            var existingParentNotes = context.VideoNotes.
                Where(x => existingParentNotesIds.Contains(x.Id)).
                ToList();

            newPageNotes = newPageNotes.OrderBy(x => x.InPageId).ToArray();
            var dbNotesToBe = new List<VideoNote>();
            for (int i = 0; i < newPageNotes.Length; i++)
            {
                var pageNote = newPageNotes[i];
                var newNote = Mapper.Instance.Map<VideoNote>(pageNote);
                newNote.VideoId = videoId;

                dbNotesToBe.Add(newNote);
            }

            for (int i = 0; i < dbNotesToBe.Count; i++)
            {
                var dbNoteToBe = dbNotesToBe[i];
                var pageNote = newPageNotes[i];

                if (pageNote.ParentDbId > 0)
                {
                    existingParentNotes.SingleOrDefault(x => x.Id == pageNote.ParentDbId)
                        .ChildNotes.Add(dbNoteToBe);
                }
                ///0 means that it is not root and has parent that is not in the db, -1 means root
                else if (pageNote.ParentDbId == 0)
                {
                    var inPageParentId = pageNote.InPageParentId;
                    var pageParent = newPageNotes.FirstOrDefault(x => x.InPageId == inPageParentId);

                    ///TODO: Remove This
                    if (pageParent == null)
                    {
                        throw new Exception("You done goofed!");
                    }

                    var indexOfPageParent = Array.IndexOf(newPageNotes, pageParent);
                    var dbParent = dbNotesToBe[indexOfPageParent];
                    dbParent.ChildNotes.Add(dbNoteToBe);
                }
            }

            this.CheckTheNestringLevel(dbNotesToBe, existingParentNotes);

            context.VideoNotes.AddRange(dbNotesToBe);

            ///Final SaveChanges for the Save
            context.SaveChanges();

            ///inPageId and Order are the same thing,
            ///realigning them after putting them in 
            ///the db in case they got reaordered
            dbNotesToBe = dbNotesToBe.OrderBy(x => x.Order).ToList();
            newPageNotes = newPageNotes.OrderBy(x => x.InPageId).ToArray();

            var resultList = new List<int[]>();
            ///sending the OK status code
            resultList.Add(new int[] { 0 });
            ///mapping the new Ids to the in-page ids and 
            ///sending them to the JS so the dbId can be send 
            ///back in case of changes
            for (int i = 0; i < dbNotesToBe.Count; i++)
            {
                resultList.Add(new int[] { newPageNotes[i].InPageId, dbNotesToBe[i].Id });
            }

            return resultList.ToArray();
        }

        ///TODO: find more efficent way to check for level
        private void CheckTheNestringLevel(List<VideoNote> dbNotesToBe, List<VideoNote> existingParentNotes)
        {
            var dict = new Dictionary<VideoNote, int>();
            var allNotes = dbNotesToBe.Concat(existingParentNotes).ToArray();
            foreach (var note in allNotes)
            {
                if (FindDeepestNesting(note, dict) > 4)
                {
                    throw new BadRequestError("The notes you are trying to save are nested deeper the four levels!");
                }
            }
        }

        private int FindDeepestNesting(VideoNote note, Dictionary<VideoNote, int> dict)
        {
            if (dict.ContainsKey(note))
            {
                return dict[note];
            }

            var deepestNesting = 0;
            foreach (var child in note.ChildNotes)
            {
                var tempDeepest = FindDeepestNesting(child, dict);
                if (tempDeepest > deepestNesting)
                {
                    deepestNesting = tempDeepest;
                }
            }

            var depth = deepestNesting + 1;
            dict[note] = depth;

            return depth;
        }

        #endregion

        #region Helpers
        ///Depricated
        //private List<VideoNote> MakeNotesFromPageNotes(List<VideoNoteCreate> pageNotes)
        //{
        //    pageNotes = pageNotes.OrderBy(x => x.InPageId).ToList();
        //    var map = new Dictionary<int, int>();
        //    var finalNotes = new List<VideoNote>();
        //    for (int i = 0; i < pageNotes.Count; i++)
        //    {
        //        var pageNote = pageNotes[i];
        //        VideoNote dbNote;

        //        dbNote = new VideoNote
        //        {
        //            Content = pageNote.Content,
        //            Formatting = pageNote.Formatting,
        //            Name = "PlaceHolder",
        //            SeekTo = pageNote.SeekTo,
        //            Level = pageNote.Level,
        //            Order = pageNote.InPageId,
        //            Type = pageNote.Type,
        //        };

        //        if (pageNote.InPageParentId != null)
        //        {
        //            dbNote.Note = finalNotes[map[(int)pageNote.InPageParentId]];
        //        }

        //        map.Add(pageNote.InPageId, i);
        //        finalNotes.Add(dbNote);
        //    }

        //    return finalNotes;
        //}

        private int Level(VideoNote note)
        {
            int val;
            if (note.NoteId == null)
            {
                val = 0;
            }
            else
            {
                val = Level(note.Note);
            }
            return 1 + val;
        }
        #endregion
    }
}

