﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using hr_application;

namespace hr_application.Migrations
{
    [DbContext(typeof(HrContext))]
    [Migration("20191229150919_Version-Field")]
    partial class VersionField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("hr_application.Models.Application", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoverLetterGuid");

                    b.Property<string>("CvGuid");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.Property<Guid>("RelatedOfferId");

                    b.Property<int>("State");

                    b.Property<string>("UserId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("RelatedOfferId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("hr_application.Models.JobOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<string>("JobTitle");

                    b.Property<string>("Location");

                    b.Property<int>("MaximumSalary");

                    b.Property<int>("MinimumSalary");

                    b.Property<string>("UserId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("JobOffers");
                });

            modelBuilder.Entity("hr_application.Models.Application", b =>
                {
                    b.HasOne("hr_application.Models.JobOffer", "RelatedOffer")
                        .WithMany("Applications")
                        .HasForeignKey("RelatedOfferId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
