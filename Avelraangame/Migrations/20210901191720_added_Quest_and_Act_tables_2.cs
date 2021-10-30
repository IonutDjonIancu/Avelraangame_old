using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avelraangame.Migrations
{
    public partial class added_Quest_and_Act_tables_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActId",
                table: "Party",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<int>(type: "int", nullable: false),
                    Prologue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Epilogue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Acts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Story = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    EpisodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acts_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acts_EpisodeId",
                table: "Acts",
                column: "EpisodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acts");

            migrationBuilder.DropTable(
                name: "Quest"
                );

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropColumn(
                name: "ActId",
                table: "Party");
        }
    }
}
