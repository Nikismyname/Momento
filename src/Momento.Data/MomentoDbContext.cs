namespace Momento.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Momento.Models.CheatSheets;
    using Momento.Models.Hashtags.MappingTables;
    using Momento.Models.Codes;
    using Momento.Models.Hashtags;
    using Momento.Models.ListsToDo;
    using Momento.Models.ListsRemind;
    using Momento.Models.Videos;
    using Momento.Models.Users;
    using Momento.Models.Directories;
    using Microsoft.AspNetCore.Identity;

    public class MomentoDbContext : IdentityDbContext<User>
    {
        public MomentoDbContext(DbContextOptions<MomentoDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserSettings> UsersSettings { get; set; }

        public DbSet<Directory> Directories { get; set; }

        public DbSet<Hashtag> Hashtags { get; set; }

        public DbSet<CodeHashtag> CodesHashtags { get; set; }

        public DbSet <CodeNoteHashtag> CodeNotesHashtags { get; set; }


        public DbSet<CheatSheet> CheatSheets { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Point> Points { get; set; }


        public DbSet<Video> Videos { get; set; }

        public DbSet<VideoNote> VideoNotes { get; set; }


        public DbSet<ListRemind> ListsRemind { get; set; }

        public DbSet<ListRemindItem> ListRemindItems { get; set; }


        public DbSet<Code> Code { get; set; }

        public DbSet<CodeNote> CodeNotes { get; set; }


        public DbSet<ListToDo> ListsTodo { get; set; }

        public DbSet<ListToDoItem> ListToDoItems { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new IdentityRole { Name = "Moderator", NormalizedName = "Moderator".ToUpper() },
                new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() });

            builder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<UserSettings>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Directory>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<CheatSheet>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Topic>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Point>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Video>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VideoNote>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<ListRemind>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<ListRemindItem>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Code>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<CodeNote>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<ListToDo>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<ListToDoItem>().HasQueryFilter(x => !x.IsDeleted);

            builder.Entity<Point>(p =>
            {
                p.HasOne(x => x.ParentPoint)
                .WithMany(x => x.ChildPoints)
                .HasForeignKey(x => x.ParentPointId);
            });

            builder.Entity<VideoNote>(n =>
            {
                n.HasOne(x => x.Note)
                .WithMany(x => x.ChildNotes)
                .HasForeignKey(x=>x.NoteId);
            });

            builder.Entity<Video>(c => 
            {
                c.HasMany(x => x.Notes)
                .WithOne(x => x.Video)
                .HasForeignKey(x => x.VideoId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Directory>(d=> 
            {
                d.HasMany(x => x.Videos)
                .WithOne(x => x.Directiry)
                .HasForeignKey(x => x.DirectoryId)
                .OnDelete(DeleteBehavior.Cascade);

                d.HasMany(x=>x.Subdirectories)
                .WithOne(x=>x.ParentDirectory)
                .HasForeignKey(x=>x.ParentDirectoryId);
            });

            builder.Entity<CodeHashtag>(c =>
            {
                c.HasKey(x => new { x.CodeId, x.HashtagId });

                c.HasOne(x => x.Code)
                .WithMany(x => x.CodeHashtags)
                .HasForeignKey(x => x.CodeId);

                c.HasOne(x => x.Hashtag)
                .WithMany(x => x.CodeHashtags)
                .HasForeignKey(x => x.HashtagId);
            });

            builder.Entity<CodeNoteHashtag>(c =>
            {
                c.HasKey(x => new { x.CodeNoteId, x.HashtagId });

                c.HasOne(x => x.CodeNote)
                .WithMany(x => x.CodeNoteHashtags)
                .HasForeignKey(x => x.CodeNoteId);

                c.HasOne(x => x.Hashtag)
                .WithMany(x => x.CodeNoteHashtags)
                .HasForeignKey(x => x.HashtagId);
            });

            builder.Entity<Hashtag>(h=> {
                h.HasIndex(x => x.Name)
                .IsUnique(true);
            });

            builder.Entity<User>(u => {
                u.HasOne(x => x.UserSettings)
                .WithOne(x => x.User)
                .HasForeignKey<UserSettings>(x => x.UserId);

                u.HasMany(x => x.Videos)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

                u.HasMany(x => x.ListsToDo)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            });

            base.OnModelCreating(builder);
        }
    }
}
