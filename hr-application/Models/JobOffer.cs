using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace hr_application.Models
{
    public class JobOffer
    {
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
