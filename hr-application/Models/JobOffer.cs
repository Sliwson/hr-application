using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace hr_application.Models
{
    public class JobOffer
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string JobTitle { get; set; }

        public string Description { get; set; }

        public int MinimumSalary { get; set; }

        public int MaximumSalary { get; set; }

        public string Location { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime ExpirationDate { get; set; }
        
        public ICollection<Application> Applications { get; set; }
    }
}
