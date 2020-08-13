using Microsoft.EntityFrameworkCore.Migrations;

namespace Reader.API.DataAccess.Migrations
{
    public partial class OptionsLogAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialAccelaretionTimeSecs",
                table: "Options");

            migrationBuilder.AddColumn<int>(
                name: "SavedLocation",
                table: "Readings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlowIfLonger",
                table: "OptionsLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlowTo",
                table: "OptionsLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InitialAccelerationTimeSecs",
                table: "Options",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlowIfLonger",
                table: "Options",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlowTo",
                table: "Options",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavedLocation",
                table: "Readings");

            migrationBuilder.DropColumn(
                name: "SlowIfLonger",
                table: "OptionsLogs");

            migrationBuilder.DropColumn(
                name: "SlowTo",
                table: "OptionsLogs");

            migrationBuilder.DropColumn(
                name: "InitialAccelerationTimeSecs",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "SlowIfLonger",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "SlowTo",
                table: "Options");

            migrationBuilder.AddColumn<int>(
                name: "InitialAccelaretionTimeSecs",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
