namespace Momento.Tests.Sandbox
{
    using Momento.Models.Enums;
    using Momento.Models.Videos;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using NUnit.Framework;
    using System.Collections.Generic;

    public class TestTest : BaseTestsSqliteInMemory
    {
        [Test]
        public void Test()
        {
            ///GOSHO
            VideoS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideosToUser(this.context, VideoS.GoshoId);

            var note1 = new VideoNote
            {
                Content = "test",
                Formatting = Formatting.CSharp,
                Level = 1,
                Order = 0,
                VideoId = video.Id,
                Type = VideoNoteType.Note,
            };

            var note2 = new VideoNote
            {
                Content = "test2",
                Formatting = Formatting.CSharp,
                Level = 1,
                Order = 0,
                VideoId = video.Id,
                Type = VideoNoteType.Note,
            };

            note1.ChildNotes.Add(note2);

            var allNotes = new List<VideoNote> {note1, note2};

            context.VideoNotes.AddRange(allNotes);
            context.SaveChanges();
        }
    }
}
