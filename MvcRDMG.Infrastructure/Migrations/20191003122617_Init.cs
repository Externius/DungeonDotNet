using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcRDMG.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DungeonName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    DungeonSize = table.Column<int>(nullable: false),
                    DungeonDifficulty = table.Column<int>(nullable: false),
                    PartyLevel = table.Column<int>(nullable: false),
                    PartySize = table.Column<int>(nullable: false),
                    TreasureValue = table.Column<double>(nullable: false),
                    ItemsRarity = table.Column<int>(nullable: false),
                    RoomDensity = table.Column<int>(nullable: false),
                    RoomSize = table.Column<int>(nullable: false),
                    MonsterType = table.Column<string>(nullable: true),
                    TrapPercent = table.Column<int>(nullable: false),
                    DeadEnd = table.Column<bool>(nullable: false),
                    Corridor = table.Column<bool>(nullable: false),
                    RoamingPercent = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DungeonTiles = table.Column<string>(nullable: true),
                    RoomDescription = table.Column<string>(nullable: true),
                    TrapDescription = table.Column<string>(nullable: true),
                    RoamingMonsterDescription = table.Column<string>(nullable: true),
                    OptionId = table.Column<int>(nullable: true)
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
                name: "IX_Options_UserId",
                table: "Options",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_DungeonName_UserId",
                table: "Options",
                columns: new[] { "DungeonName", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedDungeons_OptionId",
                table: "SavedDungeons",
                column: "OptionId");

            migrationBuilder.Sql(
                @"
                CREATE TRIGGER SetUserTimestampOnUpdate
                AFTER UPDATE ON Users
                BEGIN
                    UPDATE Users
                    SET Timestamp = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END
            ");

            migrationBuilder.Sql(
                @"
                CREATE TRIGGER SetUserTimestampOnInsert
                AFTER INSERT ON Users
                BEGIN
                    UPDATE Users
                    SET Timestamp = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END
            ");
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
