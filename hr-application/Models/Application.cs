using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hr_application.Models
{
    public class Application : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [NotMapped]
        public IFormFile CoverLetterFile { get; set; }

        [Required]
        [NotMapped]
        public IFormFile CVFile { get; set; }

        public int RelatedOfferId { get; set; }
        public JobOffer RelatedOffer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsEmailValid(Email))
                yield return new ValidationResult("Email is not valid");

            if (CoverLetterFile != null && !IsFileValid(CoverLetterFile))
                yield return new ValidationResult("Cover letter file must be in .pdf format and have size under 12MB");

            if (!IsFileValid(CVFile))
                yield return new ValidationResult("CV file must be in .pdf format and have size under 12MB");

        }

        private bool IsEmailValid(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsFileValid(IFormFile file)
        {
            const int MaxFileLength = 1024 * 1024 * 12;
            if (file.Length > MaxFileLength)
                return false;

            if (!file.FileName.EndsWith("pdf"))
                return false;

            return true;
        }
    }
}
