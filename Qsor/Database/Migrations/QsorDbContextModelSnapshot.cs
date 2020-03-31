﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Qsor.Database;

namespace Qsor.Database.Migrations
{
    [DbContext(typeof(QsorDbContext))]
    partial class QsorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("Qsor.Database.Models.BeatmapModel", b =>
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
