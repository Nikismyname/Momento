namespace Momento.Services.Implementations.Video
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Momento.Data.Models.Enums;
    using Momento.Data.Models.Videos;
    using Momento.Services.Contracts.Video;
    using Momento.Services.Models.Video;

    public class VideoService : IVideoService
    {
        private readonly MomentoDbContext context;
        private readonly IMapper mapper;

        public VideoService(MomentoDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        #region Get
        public Video ById(int id)
        => context.Videos.SingleOrDefault(x => x.Id == id);

        public VideoView GetView(int contentId)
        {
            var content = context.Videos
            .Include(x => x.Notes)
            .SingleOrDefault(x => x.Id == contentId);

            var contentView = mapper.Map<VideoView>(content);

            return contentView;
        }
        #endregion

        public int Create(int dirId)
        {
            var ordersInfo = context.Directories
                .Select(x => new { id = x.Id, orders = x.Videos.Select(y => y.Order).ToArray() })
                .SingleOrDefault(x => x.id == dirId);
            var order = ordersInfo.orders.Length == 0 ? 0 : ordersInfo.orders.Max() + 1;

            var video = new Video
            {
                DirectoryId = dirId,
                Order = order,
            };

            context.Videos.Add(video);
            context.SaveChanges();
            return video.Id;
        }

        #region Edit 
        public VideoCreate GetNotesForEdit(int contentId)
        {
            var content = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == contentId);

            var dbNotes = content.Notes.ToArray();

            var map = new Dictionary<int, int>();
            var finalNotes = new VideoNoteCreate[dbNotes.Length];

            for (int i = 0; i < dbNotes.Length; i++)
            {
                var n = dbNotes[i];
                finalNotes[i] = new VideoNoteCreate
                {
                    Id = n.Id,
                    Content = n.Content,
                    Formatting = n.Formatting,
                    Type = n.Type,
                    InPageId = i,
                    SeekTo = n.SeekTo,
                    Level = Level(n),
                };

                map.Add(n.Id, i);
            }

            for (int i = 0; i < dbNotes.Length; i++)
            {
                var n = dbNotes[i];
                var r = finalNotes[i];

                if (n.NoteId != null)
                {
                    r.InPageParentId = map[(int)n.NoteId];
                }
            }

            var result = new VideoCreate
            {
                Description = content.Description,
                Url = content.Url,
                Name = content.Name,
                DirectoryId = content.DirectoryId,
                Order = content.Order,
                SeekTo = content.SeekTo,
                Notes = finalNotes.ToList(),
            };

            return result;
        }

        public void Edit(VideoCreate model)
        {
            var contentToDelete = context.Videos.SingleOrDefault(x => x.Id == model.Id);
            var dirId = contentToDelete.DirectoryId;
            context.Videos.Remove(contentToDelete);
            context.SaveChanges();

            var finalNotes = this.MakeNotesFromPageNotes(model.Notes);

            var video = new Video
            {
                Description = model.Description,
                Name = model.Name,
                Url = model.Url,
                Order = model.Order,
                Notes = finalNotes,
                DirectoryId = dirId,
                SeekTo = model.SeekTo,
            };

            context.Videos.Add(video);
            context.SaveChanges();
        }
        #endregion

        ///Soft Delete
        public void Delete(int id)
        {
            var now = DateTime.UtcNow;

            var video = context.Videos.SingleOrDefault(x => x.Id == id);
            foreach (var note in video.Notes)
            {
                note.DeletedOn = now;
                note.IsDeleted = true;
            }

            video.DeletedOn = now;
            video.IsDeleted = true;

            context.SaveChanges();
        }

        #region PartialSave
        ///Code Map: if you send back a jagged array with first element:
        ///0 - everything is ok
        ///1 - the video does not belong to the user
        ///2 - a note to change does not belong the video
        ///3 - newItems can not be parsed
        ///4 - videoDoesNotExist
        public int[][] PartialSave(int videoId, string userName, int? seekTo, string name, string desctiption, string url, string[][] changes, VideoNoteCreate[] newNotes)
        {
            ///chech if video exists 
            var video = context.Videos.SingleOrDefault(x => x.Id == videoId);
            if (video == null)
            {
                return new int[][] { new int[] { 4 } };
            }

            var user = context.Users.
                Include(x => x.Directories)
                .SingleOrDefault(x => x.UserName == userName);

            ///check if the video belongs to the user 
            var userVideoIds = context.Users
                .Select(x => new
                {
                    username = x.UserName,
                    videoIds = x.Directories.SelectMany(y => y.Videos.Select(z => z.Id)).ToArray()
                })
                .SingleOrDefault(x => x.username == userName)
                .videoIds;

            if (!userVideoIds.Contains(videoId))
            {
                return new int[][] { new int[] { 1 } };
            }
            ///check if all the notes being changed belong the video they are coming for;
            var videoNotesIds = context
                .Videos
                .Include(x=>x.Notes)
                .SingleOrDefault(x => x.Id == videoId)
                .Notes.Select(x => x.Id).ToArray();
            var sentVideoNoteIds = changes.Select(x => int.Parse(x[0])).ToArray();

            if (sentVideoNoteIds.Any(x => !videoNotesIds.Contains(x)))
            {
                return new int[][] { new int[] { 2 } };
            }

            ///appy changes to the video fields
            this.PartialSaveVideoFields(videoId, url, name, desctiption, seekTo);
            this.PartialSaveChanges(changes);
            var result = this.PartialSaveNewItems(newNotes, videoId);
            return result;
        }
        private void PartialSaveVideoFields(int videoId, string url, string name, string description, int? seekTo)
        {
            var video = context.Videos.SingleOrDefault(x => x.Id == videoId);
            if (url != null)
            {
                video.Url = url;
            }
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
        private bool PartialSaveChanges(string[][] changes)
        {
            if (changes.Length == 0)
            {
                return true;
            }

            var videoIds = changes.Select(x => int.Parse(x[0]));
            var notesToChange = context.VideoNotes.Where(x => videoIds.Contains(x.Id)).ToArray();

            foreach (var change in changes)
            {
                var noteId = int.Parse(change[0]);
                var prop = change[1];
                var newVal = change[2];
                VideoNote currentNote = notesToChange.SingleOrDefault(x => x.Id == noteId);

                switch (prop)
                {
                    case "Deleted":
                        currentNote.IsDeleted = true;
                        currentNote.DeletedOn = DateTime.UtcNow;
                        break;
                    case "Content":
                        currentNote.Content = newVal;
                        break;
                    case "Formatting":
                        currentNote.Formatting = (Formatting)int.Parse(newVal);
                        break;
                    //case "Name":
                    //    ///not in use currently
                    //    currentNote.Name = newVal;
                    //    break;
                    case "InPageId":
                        currentNote.Order = int.Parse(newVal);
                        break;
                    case "SeekTo":
                        currentNote.SeekTo = int.Parse(newVal);
                        break;
                    ///Type does not currently change much
                }
            }
            context.SaveChanges();
            return true;
        }
        private int[][] PartialSaveNewItems(VideoNoteCreate[] newNotes, int videoId)
        {
            ///removing items which are created and deleted in the same save window
            newNotes = newNotes.Where(x => x.Deleted == false).ToArray();

            var existingParentNotesIds = newNotes
                .Select(x => x.ParentDbId)
                .Where(x => x > 0)
                .ToArray();

            var existingParentNotes = context.VideoNotes.Where(x => existingParentNotesIds.Contains(x.Id)).ToArray();

            newNotes = newNotes.OrderBy(x => x.InPageId).ToArray();
            var dbNotesToBe = new List<VideoNote>();
            for (int i = 0; i < newNotes.Length; i++)
            {
                var pn = newNotes[i];
                var newNote = new VideoNote()
                {
                    Content = pn.Content,
                    Formatting = pn.Formatting,
                    Level = pn.Level,
                    Name = "PlaceHolder",
                    Order = pn.InPageId,
                    VideoId = videoId,
                    Type = pn.Type,
                    SeekTo = pn.SeekTo,
                };
                dbNotesToBe.Add(newNote);
            }

            for (int i = 0; i < dbNotesToBe.Count; i++)
            {
                var dbNoteToBe = dbNotesToBe[i];
                var pageNote = newNotes[i];

                if (pageNote.ParentDbId > 0)
                {
                    existingParentNotes.SingleOrDefault(x => x.Id == pageNote.ParentDbId)
                        .ChildNotes.Add(dbNoteToBe);
                }
                ///0 means that it is not root and has parent that is not in the db, -1 means root
                else if (pageNote.ParentDbId == 0)
                {
                    var inPageParentId = pageNote.InPageParentId;
                    var pageParent = newNotes.FirstOrDefault(x => x.InPageId == inPageParentId);

                    //TODO: Remove This
                    if (pageParent == null)
                    {
                        throw new Exception("You done goofed!");
                    }

                    var indexOfPageParent = Array.IndexOf(newNotes, pageParent);
                    var dbParent = dbNotesToBe[indexOfPageParent];
                    dbParent.ChildNotes.Add(dbNoteToBe);
                }
            }

            context.VideoNotes.AddRange(dbNotesToBe);
            context.SaveChanges();

            ///inPageId and Order are the same thing,
            ///realigning them after putting them in 
            ///the db in case they got reaordered
            dbNotesToBe = dbNotesToBe.OrderBy(x => x.Order).ToList();
            newNotes = newNotes.OrderBy(x => x.InPageId).ToArray();

            var resultList = new List<int[]>();
            ///sending the OK status code
            resultList.Add(new int[] {0});
            ///mapping the new Ids to the in-page ids and 
            ///sending them to the JS so the dbId can be send 
            ///back in case of changes
            for (int i = 0; i < dbNotesToBe.Count; i++)
            {
                resultList.Add(new int[] { newNotes[i].InPageId, dbNotesToBe[i].Id});
            }

            return resultList.ToArray();
        }
        #endregion

        #region Helpers
        private List<VideoNote> MakeNotesFromPageNotes(List<VideoNoteCreate> pageNotes)
        {
            pageNotes = pageNotes.OrderBy(x => x.InPageId).ToList();
            var map = new Dictionary<int, int>();
            var finalNotes = new List<VideoNote>();
            for (int i = 0; i < pageNotes.Count; i++)
            {
                var pageNote = pageNotes[i];
                VideoNote dbNote;

                dbNote = new VideoNote
                {
                    Content = pageNote.Content,
                    Formatting = pageNote.Formatting,
                    Name = "PlaceHolder",
                    SeekTo = pageNote.SeekTo,
                    Level = pageNote.Level,
                    Order = pageNote.InPageId,
                    Type = pageNote.Type,
                };

                if (pageNote.InPageParentId != null)
                {
                    dbNote.Note = finalNotes[map[(int)pageNote.InPageParentId]];
                }

                map.Add(pageNote.InPageId, i);
                finalNotes.Add(dbNote);
            }

            return finalNotes;
        }

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

        #region OldCode
        public void Create(VideoCreate model)
        {
            var finalNotes = this.MakeNotesFromPageNotes(model.Notes);
            //TODO: not necessary 
            var ordersInfo = context.Directories
                .Select(x => new { id = x.Id, orders = x.Videos.Select(y => y.Order).ToArray() })
                .SingleOrDefault(x => x.id == model.DirectoryId);

            var order = ordersInfo.orders.Length == 0 ? 0 : ordersInfo.orders.Max() + 1;

            var video = new Video
            {
                Description = model.Description,
                Name = model.Name,
                Url = model.Url,
                Notes = finalNotes,
                DirectoryId = model.DirectoryId,
                Order = order,
                SeekTo = model.SeekTo,
            };

            context.Videos.Add(video);
            context.SaveChanges();
        }
        #endregion;
    }
}

