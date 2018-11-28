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

        public static string DefaultNoteContent = "TestNoteContent";
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
            var directoryId = user.Directories.FirstOrDefault(x=>x.Name.Contains("Root")).Id;

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
                         //Id = 1,
                         Content = DefaultNoteContent,
                         Formatting = DefaultNoteFormatting,
                         SeekTo = DefaultNoteSeekTo,
                    },
                    new VideoNote
                    {
                        //Id = 2,
                        Content = DefaultNoteContent,
                        Formatting = DefaultNoteFormatting,
                        SeekTo = DefaultNoteSeekTo,
                    },
                }
            };

            user.Videos.Add(video);
            context.SaveChanges();
            return video;
        }

        /// TODO: Check if you should update the The ParentDbId of notes 
        public static VideoNoteCreate[] GenerateNoteCreateSimpleNested()
        {
            var notes = new VideoNoteCreate[]
            {
                new VideoNoteCreate
                {
                    Content = "RootNote",
                    InPageId  = 0,
                    InPageParentId = null,
                    ParentDbId = -1,
                    Formatting = Formatting.None,
                    Type = VideoNoteType.Note,
                    Level = 1,
                    SeekTo = 5,
                },

                new VideoNoteCreate
                {
                    Content = "NestedLevel2",
                    InPageId  = 1,
                    InPageParentId = 0,
                    ParentDbId = 0,
                    Formatting = Formatting.None,
                    Type = VideoNoteType.Note,
                    Level = 2,
                    SeekTo = 6,
                },

                new VideoNoteCreate
                {
                    Content = "NestedLevel3",
                    InPageId  = 2,
                    InPageParentId = 1,
                    ParentDbId = 0,
                    Formatting = Formatting.None,
                    Type = VideoNoteType.Note,
                    Level = 3,
                    SeekTo = 7,
                }
            };

            return notes;
        }
    }
}

