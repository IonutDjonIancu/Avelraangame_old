using Microsoft.EntityFrameworkCore.Migrations;

namespace Avelraangame.Migrations
{
    public partial class added_Symbol_to_PlayerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastLogin",
                table: "Players",
                newName: "CreationDate");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Players",
                newName: "LastLogin");
        }
    }
}
