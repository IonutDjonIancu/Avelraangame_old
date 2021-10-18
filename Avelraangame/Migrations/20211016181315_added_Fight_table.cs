using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avelraangame.Migrations
{
    public partial class added_Fight_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActId",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "InFight",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "InParty",
                table: "Characters",
                newName: "IsInParty");

            migrationBuilder.AddColumn<bool>(
                name: "IsPartyLocked",
                table: "Party",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInFight",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPartyLocked",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "IsInFight",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "IsInParty",
                table: "Characters",
                newName: "InParty");

            migrationBuilder.AddColumn<Guid>(
                name: "ActId",
                table: "Party",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InFight",
                table: "Characters",
                type: "bit",
                nullable: true);
        }
    }
}
