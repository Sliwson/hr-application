using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using hr_application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;

namespace hr_tests
{
    class ApplicationTests
    {
        [Test]
        public void ProperModel()
        {
            var application = GetProperApplication();
            Assert.IsTrue(GetValidationResult(application).Item1);
        }

        [Test]
        public void NullCV()
        {
            var application = GetProperApplication();
            application.CVFile = null;
            Assert.IsFalse(GetValidationResult(application).Item1);
        }

        [Test]
        public void WrongEmails()
        {
            var application = GetProperApplication();
            application.Email = "test.test.test.test";
            Assert.IsFalse(GetValidationResult(application).Item1);
            
            application.Email = "test.test.test.test@";
            Assert.IsFalse(GetValidationResult(application).Item1);

            application.Email = "$$$--test.test.test.test";
            Assert.IsFalse(GetValidationResult(application).Item1);
            
            application.Email = "test.test.test@ test";
            Assert.IsFalse(GetValidationResult(application).Item1);
        }

        [Test]
        public void WrongPhoneNumber()
        {
            var application = GetProperApplication();
            //TODO: add phone number validation
            Assert.Pass();
        }

        private ApplicationFormViewModel GetProperApplication()
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(Resources.CV);
            writer.Flush();
            IFormFile file = new FormFile(ms, 0, ms.Length, "CVFile", "CV.pdf");

            return new ApplicationFormViewModel
            {
                Email = "test@test.pl",
                FirstName = "Jack",
                LastName = "Sparrow",
                PhoneNumber = "500500500",
                RelatedOfferId = Guid.NewGuid(),
                CVFile = file
            };
        }

        private (bool, List<ValidationResult>) GetValidationResult(ApplicationFormViewModel application)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(application, null, null);
            bool isValid = Validator.TryValidateObject(application, context, results, true);
            return (isValid, results);
        }
    }
}
