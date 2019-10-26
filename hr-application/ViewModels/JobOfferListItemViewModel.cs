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
        public JobOfferListItemViewModel(string jobTitle, string salaryRange, string location, DateTime expirationDate)
        {
            JobTitle = jobTitle;
            SalaryRange = salaryRange;
            Location = location;
            ExpirationDate = expirationDate;
        }

        public string JobTitle { get; set; }

        public string SalaryRange { get; set; }

        public string Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
    }
}
