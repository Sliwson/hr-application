using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Services;

namespace hr_application.Models
{
    public class User
    {
        public string Identifier { get; set; }
        public  UserRole Role { get; set; }
        public string Email { get; set; }
        public ICollection<Application> Applications { get; set; }
        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
