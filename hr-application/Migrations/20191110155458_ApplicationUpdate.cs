using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_application.Migrations
{
    public partial class ApplicationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Applications",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Applications",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Applications",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Applications",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.DropPrimaryKey("PK_Applications", table: "Applications");
            migrationBuilder.DropColumn("Id", table: "Applications");
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Applications",
                nullable: false);

            migrationBuilder.AddPrimaryKey("PK_Applications", table: "Applications", column: "Id");
            
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Applications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Applications",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Applications",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Applications",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Applications",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.DropPrimaryKey("PK_Applications", table: "Applications");
            migrationBuilder.DropColumn("Id", table: "Applications");
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Applications",
                nullable: false);

            migrationBuilder.AddPrimaryKey("PK_Applications", table: "Applications", column: "Id");
        }
    }
}
