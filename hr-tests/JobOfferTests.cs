using NUnit.Framework;
using hr_application.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using System.Linq;

namespace hr_tests
{
    public class JobOfferTests 
    {
        [Test]
        public void ProperModel()
        {
            var jobOffer = GetProperOffer();
            Assert.IsTrue(GetValidationResult(jobOffer).Item1);
        }

        [Test]
        public void EmptyFields()
        {
            var jobOffer = new JobOfferFormViewModel();
            var (result, errors) = GetValidationResult(jobOffer);
            Assert.IsFalse(result);
        }

        [Test] 
        public void Salary()
        {
            var jobOffer = GetProperOffer();
            jobOffer.MinimumSalary = 2000;
            Assert.IsFalse(GetValidationResult(jobOffer).Item1);

            jobOffer.MaximumSalary = 1000001;
            Assert.IsFalse(GetValidationResult(jobOffer).Item1);

            jobOffer.MaximumSalary = 1000;
            jobOffer.MinimumSalary = -1;
            Assert.IsFalse(GetValidationResult(jobOffer).Item1);

            jobOffer.MinimumSalary = 0;
            Assert.IsTrue(GetValidationResult(jobOffer).Item1);
        }

        [Test]
        public void LongFields()
        {
            var jobOffer = GetProperOffer();
            jobOffer.Description = String.Concat(Enumerable.Repeat("C", 1001));
            jobOffer.JobTitle = String.Concat(Enumerable.Repeat("C", 61));
            jobOffer.Location = String.Concat(Enumerable.Repeat("C", 101));
            var (result, errors) = GetValidationResult(jobOffer);
            Assert.IsFalse(result);
            Assert.IsTrue(errors.Count == 3);
        }

        [Test]
        public void IncorrectDate()
        {
            var jobOffer = GetProperOffer();
            jobOffer.ExpirationDate = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            Assert.IsFalse(GetValidationResult(jobOffer).Item1);
        }

        private JobOfferFormViewModel GetProperOffer()
        {
            return new JobOfferFormViewModel
            {
                JobTitle = "Developer",
                Description = "Good dev",
                MinimumSalary = 100,
                MaximumSalary = 200,
                Location = "Warsaw",
                ExpirationDate = DateTime.Now.AddDays(1)
            };
        }

        private (bool, List<ValidationResult>) GetValidationResult(JobOfferFormViewModel jobOffer)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(jobOffer, null, null);
            bool isValid = Validator.TryValidateObject(jobOffer, context, results, true);
            return (isValid, results);
        }
    }
}