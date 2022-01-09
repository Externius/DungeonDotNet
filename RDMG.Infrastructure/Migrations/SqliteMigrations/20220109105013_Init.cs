﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDMG.Infrastructure.Migrations.SqliteMigrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DungeonOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DungeonName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DungeonSize = table.Column<int>(type: "INTEGER", nullable: false),
                    DungeonDifficulty = table.Column<int>(type: "INTEGER", nullable: false),
                    PartyLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    PartySize = table.Column<int>(type: "INTEGER", nullable: false),
                    TreasureValue = table.Column<double>(type: "REAL", nullable: false),
                    ItemsRarity = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomDensity = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomSize = table.Column<int>(type: "INTEGER", nullable: false),
                    MonsterType = table.Column<string>(type: "TEXT", nullable: true),
                    TrapPercent = table.Column<int>(type: "INTEGER", nullable: false),
                    DeadEnd = table.Column<bool>(type: "INTEGER", nullable: false),
                    Corridor = table.Column<bool>(type: "INTEGER", nullable: false),
                    RoamingPercent = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DungeonOptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dungeons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DungeonTiles = table.Column<string>(type: "TEXT", nullable: true),
                    RoomDescription = table.Column<string>(type: "TEXT", nullable: true),
                    TrapDescription = table.Column<string>(type: "TEXT", nullable: true),
                    RoamingMonsterDescription = table.Column<string>(type: "TEXT", nullable: true),
                    DungeonOptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dungeons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dungeons_DungeonOptions_DungeonOptionId",
                        column: x => x.DungeonOptionId,
                        principalTable: "DungeonOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DungeonOptions_DungeonName_UserId",
                table: "DungeonOptions",
                columns: new[] { "DungeonName", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DungeonOptions_UserId",
                table: "DungeonOptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dungeons_DungeonOptionId",
                table: "Dungeons",
                column: "DungeonOptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dungeons");

            migrationBuilder.DropTable(
                name: "DungeonOptions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}