using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avelraangame.Migrations
{
    public partial class added_Fight_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoodGuys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BadGuys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FightDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fights", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fights");
        }
    }
}
