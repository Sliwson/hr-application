using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Models;

namespace hr_application.ViewModels
{
    public class JobOfferListItemViewModel
    {
        public JobOfferListItemViewModel(JobOffer jobOffer)
        {
            Id = jobOffer.Id;
            JobTitle = jobOffer.JobTitle;
            Location = jobOffer.Location;
            ExpirationDate = jobOffer.ExpirationDate;

            if (jobOffer.MinimumSalary == jobOffer.MaximumSalary)
                Salary = $"{jobOffer.MinimumSalary}";
            else
                Salary = $"{jobOffer.MinimumSalary} - {jobOffer.MaximumSalary}";
        }
        
        public Guid Id { get; set; }
        public string JobTitle { get; set; }

        public string Salary { get; set; }

        public string Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
    }
}
