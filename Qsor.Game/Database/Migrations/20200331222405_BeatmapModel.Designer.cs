﻿// <auto-generated />

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Qsor.Game.Database.Migrations
{
    [DbContext(typeof(QsorDbContext))]
    [Migration("20200331222405_BeatmapModel")]
    partial class BeatmapModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("Qsor.Game.Database.Models.BeatmapModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Audio")
                        .HasColumnType("TEXT");

                    b.Property<string>("File")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("beatmaps");
                });
#pragma warning restore 612, 618
        }
    }
}