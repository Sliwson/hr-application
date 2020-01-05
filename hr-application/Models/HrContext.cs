using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using hr_application.Models;

namespace hr_application
{
    public class HrContext : DbContext
    {
        public HrContext(DbContextOptions<HrContext> options)
        : base(options) { }

        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //user
            builder.Entity<User>(entity =>
            {
                entity.HasKey(s => s.Identifier);

                entity.Property(s => s.Email).IsRequired();
                entity.Property(s => s.Role).IsRequired();
            });

            //job offer
            builder.Entity<JobOffer>(entity =>
            {
                entity.HasKey(j => j.Id);
                entity.HasOne(j => j.User).WithMany(u => u.JobOffers).HasForeignKey(j => j.UserId);

                entity.Property(j => j.Description).IsRequired();
                entity.Property(j => j.ExpirationDate).IsRequired();
                entity.Property(j => j.JobTitle).IsRequired();
                entity.Property(j => j.Location).IsRequired();
                entity.Property(j => j.MaximumSalary).IsRequired();
                entity.Property(j => j.MinimumSalary).IsRequired();
                entity.Property(j => j.UserId).IsRequired();
                entity.Property(j => j.Version).IsRequired();
            });

            //application
            builder.Entity<Application>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasOne(a => a.User).WithMany(u => u.Applications).HasForeignKey(a => a.UserId);
                entity.HasOne(a => a.RelatedOffer).WithMany(j => j.Applications).HasForeignKey(j => j.RelatedOfferId);

                entity.Property(a => a.CvGuid).IsRequired();
                entity.Property(a => a.Email).IsRequired();
                entity.Property(a => a.FirstName).IsRequired();
                entity.Property(a => a.LastName).IsRequired();
                entity.Property(a => a.PhoneNumber).IsRequired();
                entity.Property(a => a.State).IsRequired();
                entity.Property(a => a.Version).IsRequired();
            });
        }
    }
}
