using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_application.Migrations
{
    public partial class BlobIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverLetterGuid",
                table: "Applications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvGuid",
                table: "Applications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverLetterGuid",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "CvGuid",
                table: "Applications");
        }
    }
}
