﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using musicShare.Models;

#nullable disable

namespace musicShare.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20231104170020_FirstCreate")]
    partial class FirstCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("musicShare.Models.Dislike", b =>
                {
                    b.Property<int>("DislikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("SongId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("DislikeId");

                    b.HasIndex("SongId");

                    b.HasIndex("UserId");

                    b.ToTable("Dislikes");
                });

            modelBuilder.Entity("musicShare.Models.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("SongId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LikeId");

                    b.HasIndex("SongId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("musicShare.Models.Song", b =>
                {
                    b.Property<int>("SongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DurMinutes")
                        .HasColumnType("int");

                    b.Property<int>("DurSeconds")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("SongId");

                    b.HasIndex("UserId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("musicShare.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("musicShare.Models.Dislike", b =>
                {
                    b.HasOne("musicShare.Models.Song", "SongDisliked")
                        .WithMany("UsersWhoDisliked")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("musicShare.Models.User", "UserWhoDisliked")
                        .WithMany("SongsDisiked")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SongDisliked");

                    b.Navigation("UserWhoDisliked");
                });

            modelBuilder.Entity("musicShare.Models.Like", b =>
                {
                    b.HasOne("musicShare.Models.Song", "SongLiked")
                        .WithMany("UsersWhoLiked")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("musicShare.Models.User", "UserWhoLiked")
                        .WithMany("SongsLiked")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SongLiked");

                    b.Navigation("UserWhoLiked");
                });

            modelBuilder.Entity("musicShare.Models.Song", b =>
                {
                    b.HasOne("musicShare.Models.User", "Submitter")
                        .WithMany("SongsSubmitted")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Submitter");
                });

            modelBuilder.Entity("musicShare.Models.Song", b =>
                {
                    b.Navigation("UsersWhoDisliked");

                    b.Navigation("UsersWhoLiked");
                });

            modelBuilder.Entity("musicShare.Models.User", b =>
                {
                    b.Navigation("SongsDisiked");

                    b.Navigation("SongsLiked");

                    b.Navigation("SongsSubmitted");
                });
#pragma warning restore 612, 618
        }
    }
}
