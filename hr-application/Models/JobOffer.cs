using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace hr_application.Models
{
    public class JobOffer : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        public string JobTitle { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0, 1000000)]
        [DataType(DataType.Currency)]
        public int MinimumSalary { get; set; }

        [Range(0, 1000000)]
        [DataType(DataType.Currency)]
        public int MaximumSalary { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
        
        public ICollection<Application> Applications { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExpirationDate <= DateTime.Now)
                yield return new ValidationResult("Expiration date must be greater than current date");

            if (MaximumSalary < MinimumSalary)
                yield return new ValidationResult("Maximum Salary must be greater or equal to Minimum Salary");
        }
    }
}
