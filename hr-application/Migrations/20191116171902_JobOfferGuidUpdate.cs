using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_application.Migrations
{
    public partial class JobOfferGuidUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "JobOffers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "JobTitle",
                table: "JobOffers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "JobOffers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000);

            migrationBuilder.DropIndex("IX_Applications_RelatedOfferId", "Applications");
            migrationBuilder.DropForeignKey("FK_Applications_JobOffers_RelatedOfferId", table: "Applications");
            migrationBuilder.DropPrimaryKey("PK_JobOffers", table: "JobOffers");
            migrationBuilder.DropColumn("Id", table: "JobOffers");
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "JobOffers",
                nullable: false);

            migrationBuilder.AddPrimaryKey("PK_JobOffers", table: "JobOffers", column: "Id");

            migrationBuilder.DropColumn("RelatedOfferId", "Applications");
            migrationBuilder.AddColumn<Guid>("RelatedOfferId", "Applications", nullable: false);

            migrationBuilder.AddForeignKey("FK_Applications_JobOffers_RelatedOfferId", table: "Applications", column: "RelatedOfferId", principalTable: "JobOffers", principalColumn: "Id");
            migrationBuilder.CreateIndex(name: "IX_Applications_RelatedOfferId", "Applications", "RelatedOfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "JobOffers",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JobTitle",
                table: "JobOffers",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "JobOffers",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.DropIndex("IX_Applications_RelatedOfferId", "Applications");
            migrationBuilder.DropForeignKey("FK_Applications_JobOffers_RelatedOfferId", table: "Applications");
            migrationBuilder.DropPrimaryKey("PK_JobOffers", table: "JobOffers");
            migrationBuilder.DropColumn("Id", table: "JobOffers");
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "JobOffers",
                nullable: false);

            migrationBuilder.AddPrimaryKey("PK_JobOffers", table: "JobOffers", column: "Id");

            migrationBuilder.DropColumn("RelatedOfferId", "Applications");
            migrationBuilder.AddColumn<int>("RelatedOfferId", "Applications", nullable: false);

            migrationBuilder.AddForeignKey("FK_Applications_JobOffers_RelatedOfferId", table: "Applications", column: "RelatedOfferId", principalTable: "JobOffers", principalColumn: "Id");
            migrationBuilder.CreateIndex(name: "IX_Applications_RelatedOfferId", "Applications", "RelatedOfferId");
        }
    }
}
