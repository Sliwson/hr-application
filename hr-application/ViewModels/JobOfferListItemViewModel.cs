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
        public JobOfferListItemViewModel(int id, string jobTitle, int minimumSalary, int maximumSalary, string location, DateTime expirationDate)
        {
            Id = id;
            JobTitle = jobTitle;
            Location = location;
            ExpirationDate = expirationDate;

            if (minimumSalary == maximumSalary)
                Salary = $"{minimumSalary}";
            else
                Salary = $"{minimumSalary} - {maximumSalary}";
        }
        
        public int Id { get; set; }
        public string JobTitle { get; set; }

        public string Salary { get; set; }

        public string Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
    }
}
