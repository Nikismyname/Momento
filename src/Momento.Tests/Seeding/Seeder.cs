namespace Momento.Tests.Seeding
{
    using Momento.Data;
    using Momento.Models.Directories;
    using Momento.Models.Enums;
    using Momento.Models.Users;
    using Momento.Models.Videos;
    using Momento.Services.Models.Video;
    using System.Collections.Generic;
    using System.Linq;

    public class Seeder
    {
        public static string PeshoId = "PeshoPeshovId";
        public static string PeshoUsername = "PeshoPeshov";
        public static string PeshoRootDir = "PeshoRoot";
        public static int PeshoRootDirId = 1;

        public static string GoshoId = "GoshoGoshovId";
        public static string GoshoUsername = "GoshoGoshov";
        public static string GoshoRootDir = "GoshoRoot";
        public static int GoshoRootDirId = 2;

        public static string DefaultVideoName = "TestVideo";
        public static string DefaultVideoUrl = "TestUrl";
        public static int DefaultVideoSeekTo = 10;
        public static string DefaultVideoDesctiption = "TestDescription";

        public static Formatting DefaultNoteFormatting = Formatting.Select_Formatting;
        public static int DefaultNoteSeekTo = 10;

        public static void SeedPeshoAndGosho(MomentoDbContext context)
        {
            var users = new User[]
            {
                new User
                {
                    Id = PeshoId,
                    FirstName = "Pesho",
                    LastName = "Peshov",
                    UserName = PeshoUsername,
                    Email = "pesho@pesho.pesho",
                    Directories = new Directory[]{ new Directory {Name = PeshoRootDir, Id = PeshoRootDirId  } }
                },
                new User
                {
                    Id = GoshoId,
                    FirstName = "Gosho",
                    LastName = "Goshov",
                    UserName = GoshoUsername,
                    Email = "gosho@gosho.gosho",
                    Directories = new Directory[]{ new Directory {Name = GoshoRootDir, Id = GoshoRootDirId  } }
                },
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }

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

        public const int preExistingNote1Id = 1;
        public const int preExistingNote2Id = 2;

        public static Video SeedVideosToUserWithNotes(MomentoDbContext context, string userId)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            var rootDirectoryId = user.Directories.FirstOrDefault(x => x.Name.Contains("Root")).Id;

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
                    new VideoNote
                    {
                         Id = preExistingNote1Id,
                         Content = preExistingNote1Content,
                         Formatting = DefaultNoteFormatting,
                         SeekTo = DefaultNoteSeekTo,
                    },
                    new VideoNote
                    {
                        Id = preExistingNote2Id,
                        Content = preExistingNote2Content,
                        Formatting = DefaultNoteFormatting,
                        SeekTo = DefaultNoteSeekTo,
                    },
                }
            };

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