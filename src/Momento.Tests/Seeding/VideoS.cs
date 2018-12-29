namespace Momento.Tests.Seeding
{
    using Momento.Data;
    using Momento.Models.Directories;
    using Momento.Models.Enums;
    using Momento.Models.Users;
    using Momento.Models.Videos;
    using Momento.Services.Models.VideoModels;
    using System.Collections.Generic;
    using System.Linq;

    public class VideoS
    {
        public static string DefaultVideoName = "TestVideo";
        public static string DefaultVideoUrl = "TestUrl";
        public static int DefaultVideoSeekTo = 10;
        public static string DefaultVideoDesctiption = "TestDescription";

        public static Formatting DefaultNoteFormatting = Formatting.Select_Formatting;
        public static int DefaultNoteSeekTo = 10;

        public static Video SeedVideosToUser(MomentoDbContext context, string userId)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            var directoryId = user.Directories.FirstOrDefault(x => x.Name.Contains("Root")).Id;

            var video = new Video
            {
                DirectoryId = directoryId,
                UserId = userId,
                Name = "TestVideo1",
            };

            context.Videos.Add(video);
            context.SaveChanges();

            return video;
        }

        public const string preExistingNote1Content = "TestContent1";
        public const string preExistingNote2Content = "TestContent2";
        public const string preExistingNote3Content = "TestContent3";

        public const int preExistingNote1Id = 1;
        public const int preExistingNote2Id = 2;
        public const int preExistingNote3Id = 3;

        public static Video SeedVideosToUserWithNotes(MomentoDbContext context, string userId, bool nestedNote = false)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            var rootDirectoryId = user.Directories.FirstOrDefault(x => x.Name.Contains("Root")).Id;

            var note1 = new VideoNote
            {
                Order = 0,
                Id = preExistingNote1Id,
                Content = preExistingNote1Content,
                Formatting = DefaultNoteFormatting,
                SeekTo = DefaultNoteSeekTo,
            };

            var note2 = new VideoNote
            {
                Order = 2,
                Id = preExistingNote2Id,
                Content = preExistingNote2Content,
                Formatting = DefaultNoteFormatting,
                SeekTo = DefaultNoteSeekTo,
            };

            var video = new Video
            {
                UserId = userId,
                Name = DefaultVideoName,
                Description = DefaultVideoDesctiption,
                SeekTo = DefaultVideoSeekTo,
                Url = DefaultVideoUrl,
                DirectoryId = rootDirectoryId,
                Notes = new HashSet<VideoNote>
                {
                    note1,
                    note2,
                }
            };

            if (nestedNote)
            {
                var note3 = new VideoNote
                {
                    Order = 1,
                    Id = preExistingNote3Id,
                    Content = preExistingNote3Content,
                    Formatting = DefaultNoteFormatting,
                    SeekTo = DefaultNoteSeekTo,
                    Video = video,
                };

                note1.ChildNotes = new HashSet<VideoNote> { note3 };
            }

            user.Videos.Add(video);
            context.SaveChanges();
            return video;
        }

        public const string Note1Content = "NestedLevel1";
        public const string Note2Content = "NestedLevel2";
        public const string Note3Content = "NestedLevel3";
        public const string Note4Content = "NestedLevel4";
        public const int Note1InPageId = 0;
        public const int Note2InPageId = 1;
        public const int Note3InPageId = 2;
        public const int Note4InPageId = 3;

        /// TODO: Check if you should update the The ParentDbId of notes 
        public static VideoNoteCreate[] GenerateNoteCreateSimpleNested(int? rootNoteDbParentId, int number = 3)
        {
            var notes = new List<VideoNoteCreate>
            {
                new VideoNoteCreate
                {
                    Content = Note1Content,
                    InPageId  = Note1InPageId,
                    InPageParentId = null,
                    ParentDbId = rootNoteDbParentId == null? -1 : rootNoteDbParentId,
                    Formatting = Formatting.None,
                    Type = VideoNoteType.Note,
                    Level = 1,
                    SeekTo = 5,
                },

                new VideoNoteCreate
                {
                    Content = Note2Content,
                    InPageId  = Note2InPageId,
                    InPageParentId = Note1InPageId,
                    ParentDbId = 0,
                    Formatting = Formatting.None,
                    Type = VideoNoteType.Note,
                    Level = 2,
                    SeekTo = 6,
                },

                new VideoNoteCreate
                {
                    Content = Note3Content,
                    InPageId  = Note3InPageId,
                    InPageParentId = Note2InPageId,
                    ParentDbId = 0,
                    Formatting = Formatting.None,
                    Type = VideoNoteType.Note,
                    Level = 3,
                    SeekTo = 7,
                }
            };

            if (number == 4)
            {
                notes.Add(new VideoNoteCreate
                {
                    Content = Note4Content,
                    InPageId = Note4InPageId,
                    InPageParentId = Note3InPageId,
                    ParentDbId = 0,
                    Formatting = Formatting.None,
                    Type = VideoNoteType.Note,
                    Level = 3,
                    SeekTo = 8,
                });
            }

            return notes.ToArray();
        }
    }
}