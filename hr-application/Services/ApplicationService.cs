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
        private readonly IUserService userService;

        public ApplicationService(HrContext context, IUserService userService)
        {
            hrContext = context;
            this.userService = userService;
        }

        public List<ApplicationListItemViewModel> GetAllApplications()
        {
            var applications = hrContext.Applications.ToList();
            return ConvertToListItems(applications);
        }

        public List<ApplicationListItemViewModel> GetUserApplications()
        {
            var userId = userService.GetUserId();
            var applications = hrContext.Applications.Where(a => a.UserId == userId).ToList();
            return ConvertToListItems(applications);
        }

        public List<ApplicationListItemViewModel> GetHrUserApplications()
        {
            var userId = userService.GetUserId();
            var applications = from application in hrContext.Applications join offer in hrContext.JobOffers.Where(o => o.UserId == userId)
                               on application.RelatedOfferId equals offer.Id select application;

            return ConvertToListItems(applications.ToList());
        }

        private List<ApplicationListItemViewModel> ConvertToListItems(List<Application> applications)
        {
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

        public bool FillJobOfferViewdata(Guid id, ViewDataDictionary viewData)
        {
            var jobOffer = hrContext.JobOffers.Find(id);
            if (jobOffer == null)
                return false;

            viewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
            return true;
        }

    }
}
