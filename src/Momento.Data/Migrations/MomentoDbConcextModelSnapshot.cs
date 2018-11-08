﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Momento.Data;

namespace Momento.Data.Migrations
{
    [DbContext(typeof(MomentoDbContext))]
    partial class MomentoDbConcextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Momento.Data.Models.CheatSheets.CheatSheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("CheatSheets");
                });

            modelBuilder.Entity("Momento.Data.Models.CheatSheets.Point", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Formatting");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentPointId");

                    b.Property<int?>("TopicId");

                    b.HasKey("Id");

                    b.HasIndex("ParentPointId");

                    b.HasIndex("TopicId");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("Momento.Data.Models.CheatSheets.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CheatSheetId");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("CheatSheetId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Momento.Data.Models.Codes.Code", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("DirectoryId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Code");
                });

            modelBuilder.Entity("Momento.Data.Models.Codes.CodeNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CodeSnippetId");

                    b.Property<string>("Content");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Formatting");

                    b.Property<string>("Hashtags");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<int>("Order");

                    b.Property<int>("WordId");

                    b.HasKey("Id");

                    b.HasIndex("CodeSnippetId");

                    b.ToTable("CodeNotes");
                });

            modelBuilder.Entity("Momento.Data.Models.Directories.Directory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<int?>("ParentDirectoryId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ParentDirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("Momento.Data.Models.Hashtags.Hashtag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Hashtags");
                });

            modelBuilder.Entity("Momento.Data.Models.Hashtags.MappingTables.CodeHashtag", b =>
                {
                    b.Property<int>("CodeId");

                    b.Property<int>("HashtagId");

                    b.HasKey("CodeId", "HashtagId");

                    b.HasIndex("HashtagId");

                    b.ToTable("CodesHashtags");
                });

            modelBuilder.Entity("Momento.Data.Models.Hashtags.MappingTables.CodeNoteHashtag", b =>
                {
                    b.Property<int>("CodeNoteId");

                    b.Property<int>("HashtagId");

                    b.HasKey("CodeNoteId", "HashtagId");

                    b.HasIndex("HashtagId");

                    b.ToTable("CodeNotesHashtags");
                });

            modelBuilder.Entity("Momento.Data.Models.ListsRemind.ListRemind", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("Momento.Data.Models.ListsRemind.ListRemindItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Importance");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<int>("ListId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ListId");

                    b.ToTable("ListItems");
                });

            modelBuilder.Entity("Momento.Data.Models.ListsToDo.ListToDo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Categories");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description");

                    b.Property<int>("DirectoryId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("ListsTodo");
                });

            modelBuilder.Entity("Momento.Data.Models.ListsToDo.ListToDoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<string>("Content");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<int>("ListToDoId");

                    b.Property<int>("Order");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ListToDoId");

                    b.ToTable("ListToDoItems");
                });

            modelBuilder.Entity("Momento.Data.Models.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Momento.Data.Models.Users.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("LACSSTheme");

                    b.Property<bool>("LADarkInputs");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("UserId");

                    b.Property<bool>("VNAutoSaveProgress");

                    b.Property<bool>("VNGoDownOnNewNoteTop");

                    b.Property<bool>("VNGoDownOnNewTimeStampTop");

                    b.Property<bool>("VNGoDownOnNewTopicTop");

                    b.Property<bool>("VNGoDownOnSubNoteAll");

                    b.Property<bool>("VNPauseVideoOnBottomNewNote");

                    b.Property<bool>("VNPauseVideoOnSubNoteRegular");

                    b.Property<bool>("VNPauseVideoOnSubNoteTop");

                    b.Property<bool>("VNPauseVideoOnTimeStampBottom");

                    b.Property<bool>("VNPauseVideoOnTimeStampTop");

                    b.Property<bool>("VNPauseVideoOnTopNewNote");

                    b.Property<bool>("VNPauseVideoOnTopicBottom");

                    b.Property<bool>("VNPauseVideoOnTopicTop");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("UsersSettings");
                });

            modelBuilder.Entity("Momento.Data.Models.Videos.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description");

                    b.Property<int>("DirectoryId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<int?>("SeekTo");

                    b.Property<string>("Url");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Momento.Data.Models.Videos.VideoNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Formatting");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<int>("Level");

                    b.Property<string>("Name");

                    b.Property<int?>("NoteId");

                    b.Property<int>("Order");

                    b.Property<int?>("SeekTo");

                    b.Property<int>("Type");

                    b.Property<int>("VideoId");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.HasIndex("VideoId");

                    b.ToTable("VideoNotes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Momento.Data.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Momento.Data.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Data.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Momento.Data.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.CheatSheets.CheatSheet", b =>
                {
                    b.HasOne("Momento.Data.Models.Users.User", "User")
                        .WithMany("CheatSheets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.CheatSheets.Point", b =>
                {
                    b.HasOne("Momento.Data.Models.CheatSheets.Point", "ParentPoint")
                        .WithMany("ChildPoints")
                        .HasForeignKey("ParentPointId");

                    b.HasOne("Momento.Data.Models.CheatSheets.Topic", "Topic")
                        .WithMany("Points")
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("Momento.Data.Models.CheatSheets.Topic", b =>
                {
                    b.HasOne("Momento.Data.Models.CheatSheets.CheatSheet", "CheatSheet")
                        .WithMany("Topics")
                        .HasForeignKey("CheatSheetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.Codes.Code", b =>
                {
                    b.HasOne("Momento.Data.Models.Users.User")
                        .WithMany("CodeSnippets")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Momento.Data.Models.Codes.CodeNote", b =>
                {
                    b.HasOne("Momento.Data.Models.Codes.Code", "CodeSnippet")
                        .WithMany("Notes")
                        .HasForeignKey("CodeSnippetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.Directories.Directory", b =>
                {
                    b.HasOne("Momento.Data.Models.Directories.Directory", "ParentDirectory")
                        .WithMany("Subdirectories")
                        .HasForeignKey("ParentDirectoryId");

                    b.HasOne("Momento.Data.Models.Users.User", "User")
                        .WithMany("Directories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.Hashtags.MappingTables.CodeHashtag", b =>
                {
                    b.HasOne("Momento.Data.Models.Codes.Code", "Code")
                        .WithMany("CodeHashtags")
                        .HasForeignKey("CodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Data.Models.Hashtags.Hashtag", "Hashtag")
                        .WithMany("CodeHashtags")
                        .HasForeignKey("HashtagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.Hashtags.MappingTables.CodeNoteHashtag", b =>
                {
                    b.HasOne("Momento.Data.Models.Codes.CodeNote", "CodeNote")
                        .WithMany("CodeNoteHashtags")
                        .HasForeignKey("CodeNoteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Data.Models.Hashtags.Hashtag", "Hashtag")
                        .WithMany("CodeNoteHashtags")
                        .HasForeignKey("HashtagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.ListsRemind.ListRemind", b =>
                {
                    b.HasOne("Momento.Data.Models.Users.User", "User")
                        .WithMany("ListsRemind")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.ListsRemind.ListRemindItem", b =>
                {
                    b.HasOne("Momento.Data.Models.ListsRemind.ListRemind", "List")
                        .WithMany("Items")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.ListsToDo.ListToDo", b =>
                {
                    b.HasOne("Momento.Data.Models.Directories.Directory", "Directory")
                        .WithMany("ListsToDo")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Data.Models.Users.User", "User")
                        .WithMany("ListsToDo")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Momento.Data.Models.ListsToDo.ListToDoItem", b =>
                {
                    b.HasOne("Momento.Data.Models.ListsToDo.ListToDo", "ListToDo")
                        .WithMany("Items")
                        .HasForeignKey("ListToDoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Data.Models.Users.UserSettings", b =>
                {
                    b.HasOne("Momento.Data.Models.Users.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("Momento.Data.Models.Users.UserSettings", "UserId");
                });

            modelBuilder.Entity("Momento.Data.Models.Videos.Video", b =>
                {
                    b.HasOne("Momento.Data.Models.Directories.Directory", "Directiry")
                        .WithMany("Videos")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Data.Models.Users.User", "User")
                        .WithMany("Videos")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Momento.Data.Models.Videos.VideoNote", b =>
                {
                    b.HasOne("Momento.Data.Models.Videos.VideoNote", "Note")
                        .WithMany("ChildNotes")
                        .HasForeignKey("NoteId");

                    b.HasOne("Momento.Data.Models.Videos.Video", "Video")
                        .WithMany("Notes")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
