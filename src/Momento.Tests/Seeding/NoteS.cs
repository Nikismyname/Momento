namespace Momento.Tests.Seeding
{
    using Momento.Data;
    using Momento.Models.Notes;
    using System.Collections.Generic;
    using System.Linq;

    public static class NoteS
    {
        const string NoteDesctiption = "noteDescription";
        const string NoteMainNoteContent = "mainNoteContent";
        const string NoteName = "singleNoteName";

        public static Note SeedNoteToUser(string userId, MomentoDbContext context)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);

            var note = new Note
            {
                Description = NoteDesctiption,
                Directory = user.Directories.First(),
                Lines = new HashSet<CodeLine>(),
                MainNoteContent = NoteMainNoteContent,
                Name = NoteName,
                EditorMode = false,
                Order = 0,
                User = user,
                ShowSourceEditor = false,
                Source = "",
            };

            context.Notes.Add(note);
            context.SaveChanges();
            return note;
        }

        const bool line1EditorMode = false;
        const int line1InPageId = 0;
        const string line1NoteContent = "line one note content";
        const bool line1Visible = false;
        const int line1Order = 0;
        const string line1SourceContent = "line one source content";

        const bool line2EditorMode = false;
        const int line2InPageId = 1;
        const string line2NoteContent = "line two note content";
        const bool line2Visible = false;
        const int line2Order = 1;
        const string line2SourceContent = "line two source content";

        public static CodeLine[] SeedTwoCodeLinesToNote(Note note, MomentoDbContext context)
        {
            var line1 = new CodeLine
            {
                EditorMode = line1EditorMode,
                InPageId = line1InPageId,
                NoteContent = line1NoteContent,
                Visible = line1Visible,
                Order = line1Order,
                SourceContent = line1SourceContent,
                Note = note,
            };

            var line2 = new CodeLine
            {
                EditorMode = line2EditorMode,
                InPageId = line2InPageId,
                NoteContent = line2NoteContent,
                Visible = line2Visible,
                Order = line2Order,
                SourceContent = line2SourceContent,
                Note = note,
            };

            var lines = new CodeLine[] { line1, line2 };

            context.CodeLines.AddRange(lines);
            context.SaveChanges();

            return lines;
        }
    }
}
