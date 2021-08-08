﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(SunriseContext))]
    partial class SunriseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("API.Entities.Customer", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("MiddleName")
                        .HasColumnType("longtext");

                    b.Property<string>("Mobile")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("API.Entities.Document", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Extension")
                        .HasColumnType("longtext");

                    b.Property<string>("JobId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("RelativePath")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("API.Entities.Job", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EndedOnUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("JobCode")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ScheduledOnUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartedOnUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("API.Entities.Document", b =>
                {
                    b.HasOne("API.Entities.Job", null)
                        .WithMany("BeforePhotos")
                        .HasForeignKey("JobId");
                });

            modelBuilder.Entity("API.Entities.Job", b =>
                {
                    b.Navigation("BeforePhotos");
                });
#pragma warning restore 612, 618
        }
    }
}
