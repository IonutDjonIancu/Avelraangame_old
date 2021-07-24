using Microsoft.EntityFrameworkCore.Migrations;

namespace Avelraangame.Migrations
{
    public partial class changed_isDraft_to_hasLevelUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDraft",
                table: "Characters",
                newName: "HasLevelup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasLevelup",
                table: "Characters",
                newName: "IsDraft");
        }
    }
}
