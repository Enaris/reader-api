using Microsoft.EntityFrameworkCore.Migrations;

namespace Reader.API.DataAccess.Migrations
{
    public partial class AccelerationTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialAccelaretionTimeSecs",
                table: "OptionsLogs");

            migrationBuilder.AddColumn<int>(
                name: "InitialAccelerationTimeSecs",
                table: "OptionsLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialAccelerationTimeSecs",
                table: "OptionsLogs");

            migrationBuilder.AddColumn<int>(
                name: "InitialAccelaretionTimeSecs",
                table: "OptionsLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
