using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace hr_application.Models
{
    public class JobOffer
    {
        public static List<JobOffer> _jobOffers = new List<JobOffer>
        {
            new JobOffer {Id = 0, JobTitle = "Developer", Description = ".NET Core MVC programmer.", MinimumSalary = 3000, MaximumSalary = 5000, ExpirationDate = new DateTime(2019,12,30), Location = "Warsaw"},
            new JobOffer {Id = 0, JobTitle = "Frontend Developer", Description = "Java Script programmer.", MinimumSalary = 3000, MaximumSalary = 5000, ExpirationDate = new DateTime(2019,12,30), Location = "Warsaw"},
            new JobOffer {Id = 0, JobTitle = "Manager", Description = "Lead of big team", MinimumSalary = 3000, MaximumSalary = 5000, ExpirationDate = new DateTime(2019,12,30), Location = "Warsaw"}
        };

        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 5)]
        public string JobTitle { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 5)]
        public string Description { get; set; }

        [Range(0, 1000000)]
        [DataType(DataType.Currency)]
        public int MinimumSalary { get; set; }

        [Range(0, 1000000)]
        [DataType(DataType.Currency)]
        public int MaximumSalary { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

    }
}
