using Microsoft.EntityFrameworkCore.Migrations;

namespace Reader.API.DataAccess.Migrations
{
    public partial class readingTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Readings",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Readings");
        }
    }
}
