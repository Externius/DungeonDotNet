using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcRDMG.Infrastructure.Migrations.SqliteMigrations
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
                name: "Options",
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DungeonTiles = table.Column<string>(type: "TEXT", nullable: true),
                    RoomDescription = table.Column<string>(type: "TEXT", nullable: true),
                    TrapDescription = table.Column<string>(type: "TEXT", nullable: true),
                    RoamingMonsterDescription = table.Column<string>(type: "TEXT", nullable: true),
                    OptionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
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
                unique: true);

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
