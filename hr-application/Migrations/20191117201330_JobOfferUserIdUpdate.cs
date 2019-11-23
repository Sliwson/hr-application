using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_application.Migrations
{
    public partial class JobOfferUserIdUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JobOffers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobOffers");
        }
    }
}
