﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RDMG.Infrastructure.Data;

#nullable disable

namespace RDMG.Infrastructure.Migrations.SqliteMigrations
{
    [DbContext(typeof(SqliteContext))]
    [Migration("20231228221034_AddTriggers")]
    partial class AddTriggers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("RDMG.Core.Domain.Dungeon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DungeonOptionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DungeonTiles")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RoamingMonsterDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("TrapDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DungeonOptionId");

                    b.ToTable("Dungeons");

                    b.HasAnnotation("Sqlite:UseSqlReturningClause", false);
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

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("DeadEnd")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DungeonDifficulty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DungeonName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DungeonSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemsRarity")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MonsterType")
                        .IsRequired()
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
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("TrapPercent")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TreasureValue")
                        .HasColumnType("REAL");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DungeonOptions");

                    b.HasAnnotation("Sqlite:UseSqlReturningClause", false);
                });

            modelBuilder.Entity("RDMG.Core.Domain.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Key")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Options");

                    b.HasAnnotation("Sqlite:UseSqlReturningClause", false);
                });

            modelBuilder.Entity("RDMG.Core.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasAnnotation("Sqlite:UseSqlReturningClause", false);
                });

            modelBuilder.Entity("RDMG.Core.Domain.Dungeon", b =>
                {
                    b.HasOne("RDMG.Core.Domain.DungeonOption", "DungeonOption")
                        .WithMany("Dungeons")
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

            modelBuilder.Entity("RDMG.Core.Domain.DungeonOption", b =>
                {
                    b.Navigation("Dungeons");
                });
#pragma warning restore 612, 618
        }
    }
}
