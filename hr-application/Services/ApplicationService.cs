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

        public List<ApplicationListItemViewModel> GetHrUserApplicationsFiltered(string query)
        {
            var userId = userService.GetUserId();
            var applications = from application in hrContext.Applications join offer in hrContext.JobOffers.Where(
                               o => o.UserId == userId && o.JobTitle.ToLower().Contains(query.ToLower()))
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

        public ServiceResult AddApplication(ApplicationFormViewModel application)
        {
            var userId = userService.GetUserId();

            var applicationEntity = new Application
            {
                Email = application.Email,
                FirstName = application.FirstName,
                LastName = application.LastName,
                PhoneNumber = application.PhoneNumber,
                RelatedOfferId = application.RelatedOfferId,
                UserId = userId,
                State = ApplicationState.Pending
            };

            hrContext.Applications.Add(applicationEntity);
            hrContext.SaveChanges();

            return ServiceResult.OK;
        }

        public ServiceResult EditApplication(Guid id, ApplicationFormViewModel application)
        {
            var foundApplication = hrContext.Applications.Find(id);
            var userId = userService.GetUserId();
            
            if (foundApplication == null)
                return ServiceResult.NotFound;
            if (foundApplication.UserId != userId)
                return ServiceResult.NotAuthorized;

            var applicationEntity = new Application
            {
                Id = foundApplication.Id,
                Email = application.Email,
                FirstName = application.FirstName,
                LastName = application.LastName,
                PhoneNumber = application.PhoneNumber,
                RelatedOfferId = application.RelatedOfferId,
                UserId = foundApplication.UserId,
                State = foundApplication.State
            };

            hrContext.Entry(foundApplication).CurrentValues.SetValues(applicationEntity);
            hrContext.SaveChanges();

            return ServiceResult.OK;
        }

        public ServiceResult ChangeApplicationState(Guid id, int state)
        {
            var application = hrContext.Applications.Find(id);
            if (application == null)
                return ServiceResult.NotFound;

            if (application.RelatedOffer.UserId != userService.GetUserId())
                return ServiceResult.NotAuthorized;

            if (!Enum.IsDefined(typeof(ApplicationState), state))
                return ServiceResult.ArgumentError;

            var applicationState = (ApplicationState)state;
            application.State = applicationState;
            hrContext.SaveChanges();

            return ServiceResult.OK;
        }

        public ServiceResult DeleteApplication(Guid id)
        {
            var application = hrContext.Applications.Find(id);
            var userId = userService.GetUserId();

            if (application == null)
                return ServiceResult.NotFound;

            if (application.UserId != userId)
                return ServiceResult.NotAuthorized;
            
            hrContext.Applications.Remove(application);
            hrContext.SaveChanges();

            return ServiceResult.OK;
        }

        public ServiceResult FillJobOfferViewdata(Guid id, ViewDataDictionary viewData)
        {
            var jobOffer = hrContext.JobOffers.Find(id);
            if (jobOffer == null)
                return ServiceResult.NotFound;

            viewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
            return ServiceResult.OK;
        }

    }
}
