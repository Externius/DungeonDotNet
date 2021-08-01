﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcRDMG.Infrastructure.Migrations.SqlServerMigrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DungeonName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DungeonSize = table.Column<int>(type: "int", nullable: false),
                    DungeonDifficulty = table.Column<int>(type: "int", nullable: false),
                    PartyLevel = table.Column<int>(type: "int", nullable: false),
                    PartySize = table.Column<int>(type: "int", nullable: false),
                    TreasureValue = table.Column<double>(type: "float", nullable: false),
                    ItemsRarity = table.Column<int>(type: "int", nullable: false),
                    RoomDensity = table.Column<int>(type: "int", nullable: false),
                    RoomSize = table.Column<int>(type: "int", nullable: false),
                    MonsterType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrapPercent = table.Column<int>(type: "int", nullable: false),
                    DeadEnd = table.Column<bool>(type: "bit", nullable: false),
                    Corridor = table.Column<bool>(type: "bit", nullable: false),
                    RoamingPercent = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Options_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedDungeons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DungeonTiles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrapDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoamingMonsterDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedDungeons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedDungeons_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Options_DungeonName_UserId",
                table: "Options",
                columns: new[] { "DungeonName", "UserId" },
                unique: true,
                filter: "[DungeonName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Options_UserId",
                table: "Options",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedDungeons_OptionId",
                table: "SavedDungeons",
                column: "OptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedDungeons");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
