using Momento.Data;
using Momento.Models.Enums;
using Momento.Models.Videos;
using Momento.Services.Contracts.Test;
using System.Collections.Generic;

namespace Momento.Services.Implementations.Test
{
    public class TestService : ITestService
    {
        private readonly MomentoDbContext context;

        public TestService(MomentoDbContext context)
        {
            this.context = context;
        }

        public void Test(int videoId)
        {
            var note1 = new VideoNote
            {
                Content = "test",
                Formatting = Formatting.CSharp,
                Level = 1,
                Order = 0,
                VideoId = videoId,
                Type = VideoNoteType.Note,
            };

            var note2 = new VideoNote
            {
                Content = "test2",
                Formatting = Formatting.CSharp,
                Level = 1,
                Order = 0,
                VideoId = videoId,
                Type = VideoNoteType.Note,
            };

            //note1.ChildNotes.Add(note2);

            var allNotes = new List<VideoNote> { note1, note2 };

            context.VideoNotes.AddRange(allNotes);
            context.SaveChanges();
        }
    }
}
