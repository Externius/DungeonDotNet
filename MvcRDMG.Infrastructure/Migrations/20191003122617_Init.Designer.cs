﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvcRDMG.Infrastructure;

namespace MvcRDMG.Infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20191003122617_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("MvcRDMG.Core.Domain.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Corridor")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<bool>("DeadEnd")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DungeonDifficulty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DungeonName")
                        .HasColumnType("TEXT");

                    b.Property<int>("DungeonSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemsRarity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MonsterType")
                        .HasColumnType("TEXT");

                    b.Property<int>("PartyLevel")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartySize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoamingPercent")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomDensity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrapPercent")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TreasureValue")
                        .HasColumnType("REAL");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("DungeonName", "UserId")
                        .IsUnique();

                    b.ToTable("Options");
                });

            modelBuilder.Entity("MvcRDMG.Core.Domain.SavedDungeon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DungeonTiles")
                        .HasColumnType("TEXT");

                    b.Property<int?>("OptionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RoamingMonsterDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("TrapDescription")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OptionId");

                    b.ToTable("SavedDungeons");
                });

            modelBuilder.Entity("MvcRDMG.Core.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MvcRDMG.Core.Domain.Option", b =>
                {
                    b.HasOne("MvcRDMG.Core.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MvcRDMG.Core.Domain.SavedDungeon", b =>
                {
                    b.HasOne("MvcRDMG.Core.Domain.Option", null)
                        .WithMany("SavedDungeons")
                        .HasForeignKey("OptionId");
                });
#pragma warning restore 612, 618
        }
    }
}
