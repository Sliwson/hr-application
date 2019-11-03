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
    }
}
