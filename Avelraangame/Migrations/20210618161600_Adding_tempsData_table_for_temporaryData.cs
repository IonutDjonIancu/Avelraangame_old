using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avelraangame.Migrations
{
    public partial class Adding_tempsData_table_for_temporaryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Temps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataType = table.Column<int>(type: "int", nullable: false),
                    DataUniqueIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temps", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Temps");
        }
    }
}
