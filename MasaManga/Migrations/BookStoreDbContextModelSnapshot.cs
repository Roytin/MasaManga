﻿// <auto-generated />
using System;
using MasaManga.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MasaManga.Migrations
{
    [DbContext(typeof(BookStoreDbContext))]
    partial class BookStoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("MasaManga.Data.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cover")
                        .HasColumnType("TEXT");

                    b.Property<string>("CoverUrl")
                        .HasColumnType("TEXT");

                    b.Property<int>("DownloadPage")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IndexUrl")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDownloading")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SourceTitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalPage")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("MasaManga.Data.BookPic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BookSectionId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DownloadTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Index")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDownloaded")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SectionIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BookSectionId");

                    b.ToTable("BookPics");
                });

            modelBuilder.Entity("MasaManga.Data.BookSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BookId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Index")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("BookSections");
                });

            modelBuilder.Entity("MasaManga.Data.BookPic", b =>
                {
                    b.HasOne("MasaManga.Data.BookSection", null)
                        .WithMany("Pics")
                        .HasForeignKey("BookSectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MasaManga.Data.BookSection", b =>
                {
                    b.HasOne("MasaManga.Data.Book", null)
                        .WithMany("Sections")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MasaManga.Data.Book", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("MasaManga.Data.BookSection", b =>
                {
                    b.Navigation("Pics");
                });
#pragma warning restore 612, 618
        }
    }
}
