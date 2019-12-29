using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_application.Migrations
{
    public partial class VersionField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "JobOffers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Applications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Applications");
        }
    }
}
