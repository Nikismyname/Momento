namespace Momento.Tests.Seeding
{
    using Momento.Data;
    using Momento.Models.Notes;
    using System.Linq;

    public static class NoteS
    {
        public const int NoteId = 1;
        public const string NoteDesctiption = "noteDescription";
        public const string NoteMainNoteContent = "mainNoteContent";
        public const string NoteName = "singleNoteName";
        public const bool NoteEditorModel = false;
        public const int NoteOrder = 0;
        public const bool NoteShowSourceEditor = false;
        public const string NoteSource = "leet source code";

        public static Note SeedNoteToUser(string userId, MomentoDbContext context, int? dirId = null)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);

            var directory = dirId == null ?
                user.Directories.Single(x => x.ParentDirectoryId == null) :
                context.Directories.Single(x => x.Id == dirId);

            var note = new Note
            {
                Id = dirId == null? NoteId : 0,
                Description = NoteDesctiption,
                Directory = directory,
                MainNoteContent = NoteMainNoteContent,
                Name = NoteName,
                EditorMode = NoteEditorModel,
                Order = NoteOrder,
                User = user,
                ShowSourceEditor = NoteShowSourceEditor,
                Source = NoteSource,
                //Lines = new HashSet<CodeLine>(),
            };

            context.Notes.Add(note);
            context.SaveChanges();
            return note;
        }

        public const int Line1Id = 1;
        public const bool Line1EditorMode = false;
        public const int Line1InPageId = 0;
        public const string Line1NoteContent = "line one note content";
        public const bool Line1Visible = false;
        public const int Line1Order = 0;
        public const string Line1SourceContent = "line one source content";

        public const int Line2Id = 2;
        public const bool Line2EditorMode = false;
        public const int Line2InPageId = 1;
        public const string Line2NoteContent = "line two note content";
        public const bool Line2Visible = false;
        public const int Line2Order = 1;
        public const string Line2SourceContent = "line two source content";

        public static CodeLine[] SeedTwoCodeLinesToNote(
            Note note, MomentoDbContext context, bool autoGenerateIds = false)
        {
            var line1 = new CodeLine
            {
                Id = autoGenerateIds == true? 0 : Line1Id,
                EditorMode = Line1EditorMode,
                InPageId = Line1InPageId,
                NoteContent = Line1NoteContent,
                Visible = Line1Visible,
                Order = Line1Order,
                SourceContent = Line1SourceContent,
                Note = note,
            };

            var line2 = new CodeLine
            {
                Id = autoGenerateIds == true ? 0 : Line2Id,
                EditorMode = Line2EditorMode,
                InPageId = Line2InPageId,
                NoteContent = Line2NoteContent,
                Visible = Line2Visible,
                Order = Line2Order,
                SourceContent = Line2SourceContent,
                Note = note,
            };

            var lines = new CodeLine[] { line1, line2 };

            context.CodeLines.AddRange(lines);
            context.SaveChanges();

            return lines;
        }
    }
}
