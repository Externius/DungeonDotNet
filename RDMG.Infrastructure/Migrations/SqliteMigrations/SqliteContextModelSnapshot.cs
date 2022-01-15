﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RDMG.Infrastructure;

#nullable disable

namespace RDMG.Infrastructure.Migrations.SqliteMigrations
{
    [DbContext(typeof(SqliteContext))]
    partial class SqliteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("RDMG.Core.Domain.Dungeon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DungeonOptionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DungeonTiles")
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RoamingMonsterDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomDescription")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("TrapDescription")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DungeonOptionId");

                    b.ToTable("Dungeons");
                });

            modelBuilder.Entity("RDMG.Core.Domain.DungeonOption", b =>
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

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

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

                    b.ToTable("DungeonOptions");
                });

            modelBuilder.Entity("RDMG.Core.Domain.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Key", "Name")
                        .IsUnique();

                    b.ToTable("Options");
                });

            modelBuilder.Entity("RDMG.Core.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RDMG.Core.Domain.Dungeon", b =>
                {
                    b.HasOne("RDMG.Core.Domain.DungeonOption", "DungeonOption")
                        .WithMany()
                        .HasForeignKey("DungeonOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DungeonOption");
                });

            modelBuilder.Entity("RDMG.Core.Domain.DungeonOption", b =>
                {
                    b.HasOne("RDMG.Core.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
