using Microsoft.EntityFrameworkCore.Migrations;

namespace Avelraangame.Migrations
{
    public partial class major_refactoring_of_ItemsAndCharacterModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetsBase",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Properties",
                table: "Items",
                newName: "Slots");

            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "Items",
                newName: "CharacterId");

            migrationBuilder.RenameColumn(
                name: "StatsRoll",
                table: "Characters",
                newName: "Wealth");

            migrationBuilder.RenameColumn(
                name: "StatsBase",
                table: "Characters",
                newName: "Logbook");

            migrationBuilder.RenameColumn(
                name: "EntityLevel",
                table: "Characters",
                newName: "Toughness");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InSlot",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bonuses",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Abstract",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Awareness",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DRM",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Strength",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bonuses",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Abstract",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Awareness",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "DRM",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Slots",
                table: "Items",
                newName: "Properties");

            migrationBuilder.RenameColumn(
                name: "CharacterId",
                table: "Items",
                newName: "Owner");

            migrationBuilder.RenameColumn(
                name: "Wealth",
                table: "Characters",
                newName: "StatsRoll");

            migrationBuilder.RenameColumn(
                name: "Toughness",
                table: "Characters",
                newName: "EntityLevel");

            migrationBuilder.RenameColumn(
                name: "Logbook",
                table: "Characters",
                newName: "StatsBase");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "InSlot",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AssetsBase",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
