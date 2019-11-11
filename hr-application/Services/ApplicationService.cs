using hr_application.Models;
using hr_application.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_application.Services
{
    public class ApplicationService
    {
        private readonly HrContext hrContext;

        public ApplicationService(HrContext context)
        {
            hrContext = context;
        }

        public List<ApplicationListItemViewModel> GetAllApplications()
        {
            var applications = hrContext.Applications.ToList();
            var applicationViewModels = new List<ApplicationListItemViewModel>();
            foreach (var app in applications)
                applicationViewModels.Add(new ApplicationListItemViewModel(app));

            return applicationViewModels;
        }

        public bool AddApplication(ApplicationFormViewModel application, string userId)
        {
            var applicationEntity = new Application
            {
                Email = application.Email,
                FirstName = application.FirstName,
                LastName = application.LastName,
                PhoneNumber = application.PhoneNumber,
                RelatedOfferId = application.RelatedOfferId,
                UserId = userId
            };

            hrContext.Applications.Add(applicationEntity);
            hrContext.SaveChanges();

            return true;
        }

        public bool EditApplication(Guid id, string userId, ApplicationFormViewModel application)
        {
            var foundApplication = hrContext.Applications.Find(id);
            if (foundApplication == null || foundApplication.UserId != userId)
                return false;

            var applicationEntity = new Application
            {
                Id = foundApplication.Id,
                Email = application.Email,
                FirstName = application.FirstName,
                LastName = application.LastName,
                PhoneNumber = application.PhoneNumber,
                RelatedOfferId = application.RelatedOfferId,
                UserId = foundApplication.UserId
            };

            hrContext.Entry(foundApplication).CurrentValues.SetValues(applicationEntity);
            hrContext.SaveChanges();

            return true;
        }

        public bool DeleteApplication(Guid id, string userId)
        {
            var application = hrContext.Applications.Find(id);
            if (application != null && application.UserId == userId)
            {
                hrContext.Applications.Remove(application);
                hrContext.SaveChanges();

                return true;            }

            return false;
        }

        public bool FillJobOfferViewdata(int id, ViewDataDictionary viewData)
        {
            var jobOffer = hrContext.JobOffers.Find(id);
            if (jobOffer == null)
                return false;

            viewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
            return true;
        }

    }
}
