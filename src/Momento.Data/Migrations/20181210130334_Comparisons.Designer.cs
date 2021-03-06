﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Momento.Data;

namespace Momento.Data.Migrations
{
    [DbContext(typeof(MomentoDbContext))]
    [Migration("20181210130334_Comparisons")]
    partial class Comparisons
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
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

                    b.HasData(
                        new { Id = "6d6cbb04-a886-41aa-a626-ae0d213e2f48", ConcurrencyStamp = "3c8539d8-0ae9-48e1-94df-aa50009ecf74", Name = "Admin", NormalizedName = "ADMIN" },
                        new { Id = "93eaadb3-1b37-4c22-9183-b7799607739f", ConcurrencyStamp = "7108f569-289d-46c0-9456-10a50468c622", Name = "Moderator", NormalizedName = "MODERATOR" },
                        new { Id = "ccff8a6c-2f54-42e9-8103-96e3f5137ce9", ConcurrencyStamp = "1aed025d-e478-4398-9c7f-a99e27d8be94", Name = "User", NormalizedName = "USER" }
                    );
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
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

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

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Momento.Models.CheatSheets.CheatSheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("CheatSheets");
                });

            modelBuilder.Entity("Momento.Models.CheatSheets.Point", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Formatting");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentPointId");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.Property<int?>("TopicId");

                    b.HasKey("Id");

                    b.HasIndex("ParentPointId");

                    b.HasIndex("TopicId");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("Momento.Models.CheatSheets.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CheatSheetId");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.HasKey("Id");

                    b.HasIndex("CheatSheetId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Momento.Models.Codes.Code", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("DirectoryId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Code");
                });

            modelBuilder.Entity("Momento.Models.Codes.CodeNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CodeSnippetId");

                    b.Property<string>("Content");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Formatting");

                    b.Property<string>("Hashtags");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<int>("Order");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.Property<int>("WordId");

                    b.HasKey("Id");

                    b.HasIndex("CodeSnippetId");

                    b.ToTable("CodeNotes");
                });

            modelBuilder.Entity("Momento.Models.Comparisons.Comparison", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description");

                    b.Property<int>("DirectoryId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<string>("SourceLanguage");

                    b.Property<string>("TargetLanguage");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Comparisons");
                });

            modelBuilder.Entity("Momento.Models.Comparisons.ComparisonItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<int>("ComparisonId");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<int>("Order");

                    b.Property<string>("Source");

                    b.Property<string>("Target");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.HasKey("Id");

                    b.HasIndex("ComparisonId");

                    b.ToTable("ComparisonItems");
                });

            modelBuilder.Entity("Momento.Models.Directories.Directory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<int?>("ParentDirectoryId");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ParentDirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("Momento.Models.Hashtags.Hashtag", b =>
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

            modelBuilder.Entity("Momento.Models.Hashtags.MappingTables.CodeHashtag", b =>
                {
                    b.Property<int>("CodeId");

                    b.Property<int>("HashtagId");

                    b.HasKey("CodeId", "HashtagId");

                    b.HasIndex("HashtagId");

                    b.ToTable("CodesHashtags");
                });

            modelBuilder.Entity("Momento.Models.Hashtags.MappingTables.CodeNoteHashtag", b =>
                {
                    b.Property<int>("CodeNoteId");

                    b.Property<int>("HashtagId");

                    b.HasKey("CodeNoteId", "HashtagId");

                    b.HasIndex("HashtagId");

                    b.ToTable("CodeNotesHashtags");
                });

            modelBuilder.Entity("Momento.Models.ListsRemind.ListRemind", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<string>("Name");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ListsRemind");
                });

            modelBuilder.Entity("Momento.Models.ListsRemind.ListRemindItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Importance");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<int>("ListId");

                    b.Property<int>("Status");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.HasKey("Id");

                    b.HasIndex("ListId");

                    b.ToTable("ListRemindItems");
                });

            modelBuilder.Entity("Momento.Models.ListsToDo.ListToDo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Categories");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description");

                    b.Property<int>("DirectoryId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<string>("Name");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("ListsTodo");
                });

            modelBuilder.Entity("Momento.Models.ListsToDo.ListToDoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<string>("Content");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<int>("ListToDoId");

                    b.Property<int>("Order");

                    b.Property<string>("Status");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.HasKey("Id");

                    b.HasIndex("ListToDoId");

                    b.ToTable("ListToDoItems");
                });

            modelBuilder.Entity("Momento.Models.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("LastName");

                    b.Property<DateTime?>("LastViewdOn");

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

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

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

            modelBuilder.Entity("Momento.Models.Users.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("LACSSTheme");

                    b.Property<bool>("LADarkInputs");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

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

            modelBuilder.Entity("Momento.Models.Videos.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description");

                    b.Property<int>("DirectoryId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<int?>("SeekTo");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

                    b.Property<string>("Url");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Momento.Models.Videos.VideoNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Formatting");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LastViewdOn");

                    b.Property<int>("Level");

                    b.Property<string>("Name");

                    b.Property<int?>("NoteId");

                    b.Property<int>("Order");

                    b.Property<int?>("SeekTo");

                    b.Property<int>("TimesModified");

                    b.Property<int>("TimesViewd");

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
                    b.HasOne("Momento.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Momento.Models.Users.User")
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

                    b.HasOne("Momento.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Momento.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.CheatSheets.CheatSheet", b =>
                {
                    b.HasOne("Momento.Models.Users.User", "User")
                        .WithMany("CheatSheets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.CheatSheets.Point", b =>
                {
                    b.HasOne("Momento.Models.CheatSheets.Point", "ParentPoint")
                        .WithMany("ChildPoints")
                        .HasForeignKey("ParentPointId");

                    b.HasOne("Momento.Models.CheatSheets.Topic", "Topic")
                        .WithMany("Points")
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("Momento.Models.CheatSheets.Topic", b =>
                {
                    b.HasOne("Momento.Models.CheatSheets.CheatSheet", "CheatSheet")
                        .WithMany("Topics")
                        .HasForeignKey("CheatSheetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.Codes.Code", b =>
                {
                    b.HasOne("Momento.Models.Users.User")
                        .WithMany("CodeSnippets")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Momento.Models.Codes.CodeNote", b =>
                {
                    b.HasOne("Momento.Models.Codes.Code", "CodeSnippet")
                        .WithMany("Notes")
                        .HasForeignKey("CodeSnippetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.Comparisons.Comparison", b =>
                {
                    b.HasOne("Momento.Models.Directories.Directory", "Directory")
                        .WithMany("Comparisons")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Models.Users.User", "User")
                        .WithMany("Comparisons")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Momento.Models.Comparisons.ComparisonItem", b =>
                {
                    b.HasOne("Momento.Models.Comparisons.Comparison", "Comparison")
                        .WithMany("Items")
                        .HasForeignKey("ComparisonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.Directories.Directory", b =>
                {
                    b.HasOne("Momento.Models.Directories.Directory", "ParentDirectory")
                        .WithMany("Subdirectories")
                        .HasForeignKey("ParentDirectoryId");

                    b.HasOne("Momento.Models.Users.User", "User")
                        .WithMany("Directories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.Hashtags.MappingTables.CodeHashtag", b =>
                {
                    b.HasOne("Momento.Models.Codes.Code", "Code")
                        .WithMany("CodeHashtags")
                        .HasForeignKey("CodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Models.Hashtags.Hashtag", "Hashtag")
                        .WithMany("CodeHashtags")
                        .HasForeignKey("HashtagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.Hashtags.MappingTables.CodeNoteHashtag", b =>
                {
                    b.HasOne("Momento.Models.Codes.CodeNote", "CodeNote")
                        .WithMany("CodeNoteHashtags")
                        .HasForeignKey("CodeNoteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Models.Hashtags.Hashtag", "Hashtag")
                        .WithMany("CodeNoteHashtags")
                        .HasForeignKey("HashtagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.ListsRemind.ListRemind", b =>
                {
                    b.HasOne("Momento.Models.Users.User", "User")
                        .WithMany("ListsRemind")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.ListsRemind.ListRemindItem", b =>
                {
                    b.HasOne("Momento.Models.ListsRemind.ListRemind", "List")
                        .WithMany("Items")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.ListsToDo.ListToDo", b =>
                {
                    b.HasOne("Momento.Models.Directories.Directory", "Directory")
                        .WithMany("ListsToDo")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Models.Users.User", "User")
                        .WithMany("ListsToDo")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Momento.Models.ListsToDo.ListToDoItem", b =>
                {
                    b.HasOne("Momento.Models.ListsToDo.ListToDo", "ListToDo")
                        .WithMany("Items")
                        .HasForeignKey("ListToDoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Momento.Models.Users.UserSettings", b =>
                {
                    b.HasOne("Momento.Models.Users.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("Momento.Models.Users.UserSettings", "UserId");
                });

            modelBuilder.Entity("Momento.Models.Videos.Video", b =>
                {
                    b.HasOne("Momento.Models.Directories.Directory", "Directiry")
                        .WithMany("Videos")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Momento.Models.Users.User", "User")
                        .WithMany("Videos")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Momento.Models.Videos.VideoNote", b =>
                {
                    b.HasOne("Momento.Models.Videos.VideoNote", "Note")
                        .WithMany("ChildNotes")
                        .HasForeignKey("NoteId");

                    b.HasOne("Momento.Models.Videos.Video", "Video")
                        .WithMany("Notes")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
