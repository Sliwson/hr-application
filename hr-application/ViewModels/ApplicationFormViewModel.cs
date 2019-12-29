using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Models;

namespace hr_application.ViewModels
{
    public class ApplicationFormViewModel : IValidatableObject
    {
        public ApplicationFormViewModel() { }
        public ApplicationFormViewModel(Application application)
        {
            Email = application.Email;
            FirstName = application.FirstName;
            LastName = application.LastName;
            PhoneNumber = application.PhoneNumber;
            RelatedOfferId = application.RelatedOfferId;
            Version = application.Version;
        }

        public int Version { get; set; }

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

        public IFormFile CoverLetterFile { get; set; }

        [Required]
        public IFormFile CVFile { get; set; }

        public Guid RelatedOfferId { get; set; }

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
