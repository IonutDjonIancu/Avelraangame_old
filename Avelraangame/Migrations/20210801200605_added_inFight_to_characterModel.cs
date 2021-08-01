using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avelraangame.Migrations
{
    public partial class added_inFight_to_characterModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FightId",
                table: "Characters",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InFight",
                table: "Characters",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FightId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InFight",
                table: "Characters");
        }
    }
}
