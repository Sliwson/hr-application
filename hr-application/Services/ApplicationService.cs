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
        private readonly StorageService storageService;

        public ApplicationService(HrContext context, IUserService userService, StorageService storageService)
        {
            hrContext = context;
            this.userService = userService;
            this.storageService = storageService;
        }

        public List<ApplicationListItemViewModel> GetApplications()
        {
            var role = userService.GetUserRole();
            if (role == UserRole.Admin)
                return GetAllApplications();
            else if (role == UserRole.Hr)
                return GetHrUserApplications();
            else if (role == UserRole.User)
                return GetUserApplications();

            return new List<ApplicationListItemViewModel>();
        }

        public List<ApplicationListItemViewModel> GetApplicationsFiltered(string query)
        {
            var list = GetApplications();
            var filtered = list.Where(a => a.Email.Contains(query) || a.FirstName.Contains(query) || a.LastName.Contains(query) || a.PhoneNumber.Contains(query));
            return filtered.ToList(); 
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
            if (query == null)
                query = "";
            
            var userId = userService.GetUserId();
            var applications = from application in hrContext.Applications join offer in hrContext.JobOffers.Where(
                               o => o.UserId == userId && o.JobTitle.ToLower().Contains(query.ToLower()))
                               on application.RelatedOfferId equals offer.Id select application;

            return ConvertToListItems(applications.ToList());
        }

        public List<ApplicationListItemViewModel> GetApplicationsForJobOffer(Guid id)
        {
            var applications = hrContext.Applications.Where(a => a.RelatedOfferId == id); 
            return ConvertToListItems(applications.ToList());
        }

        private List<ApplicationListItemViewModel> ConvertToListItems(List<Application> applications)
        {
            var applicationViewModels = new List<ApplicationListItemViewModel>();
            foreach (var app in applications)
                applicationViewModels.Add(new ApplicationListItemViewModel(app));

            return applicationViewModels;
        }

        public async Task<ServiceResult> AddApplication(ApplicationFormViewModel application)
        {
            var userId = userService.GetUserId();
            string cvPath = await storageService.StoreFile(application.CVFile);

            var applicationEntity = new Application
            {
                Version = application.Version,
                Email = application.Email,
                FirstName = application.FirstName,
                LastName = application.LastName,
                PhoneNumber = application.PhoneNumber,
                RelatedOfferId = application.RelatedOfferId,
                UserId = userId,
                State = ApplicationState.Pending,
                CvGuid = cvPath
            };

            if (application.CoverLetterFile != null)
                applicationEntity.CoverLetterGuid = await storageService.StoreFile(application.CoverLetterFile);

            hrContext.Applications.Add(applicationEntity);
            hrContext.SaveChanges();

            return ServiceResult.OK;
        }

        public async Task<ServiceResult> EditApplication(Guid id, ApplicationFormViewModel application)
        {
            var foundApplication = hrContext.Applications.Find(id);
            var userId = userService.GetUserId();
            
            if (foundApplication == null)
                return ServiceResult.NotFound;
            if (foundApplication.UserId != userId)
                return ServiceResult.NotAuthorized;
            if (foundApplication.State != ApplicationState.Pending)
                return ServiceResult.ArgumentError;
            if (foundApplication.Version != application.Version)
                return ServiceResult.SimultanousEdit;

            string cvPath = await storageService.StoreFile(application.CVFile);

            var applicationEntity = new Application
            {
                Id = foundApplication.Id,
                Version = foundApplication.Version + 1,
                Email = application.Email,
                FirstName = application.FirstName,
                LastName = application.LastName,
                PhoneNumber = application.PhoneNumber,
                RelatedOfferId = application.RelatedOfferId,
                UserId = foundApplication.UserId,
                State = foundApplication.State,
                CvGuid = cvPath,
                CoverLetterGuid = foundApplication.CoverLetterGuid
            };
            
            if (application.CoverLetterFile != null)
                applicationEntity.CoverLetterGuid = await storageService.StoreFile(application.CoverLetterFile);

            hrContext.Entry(foundApplication).CurrentValues.SetValues(applicationEntity);
            hrContext.SaveChanges();

            return ServiceResult.OK;
        }

        public ServiceResult ChangeApplicationState(Guid id, int state)
        {
            var application = hrContext.Applications.Find(id);
            if (application == null)
                return ServiceResult.NotFound;

            var jobOffer = hrContext.JobOffers.Find(application.RelatedOfferId);
            if (jobOffer == null)
                return ServiceResult.NotFound;
            if (jobOffer.UserId != userService.GetUserId())
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

        public async Task<ApplicationDetailsHrViewModel> GetHrDetails(Application application)
        {
            var model = new ApplicationDetailsHrViewModel(application);

            if (application.CvGuid != null && application.CvGuid.Length > 0)
                model.CvUrl = await storageService.GetDownloadUrl(application.CvGuid);
            else
                model.CvUrl = null;

            if (application.CoverLetterGuid != null && application.CoverLetterGuid.Length > 0)
                model.CoverLetterUrl = await storageService.GetDownloadUrl(application.CoverLetterGuid);
            else
                model.CvUrl = null;

            return model;
        }

        public ServiceResult FillJobOfferViewdata(Guid id, ViewDataDictionary viewData)
        {
            var jobOffer = hrContext.JobOffers.Find(id);
            if (jobOffer == null)
                return ServiceResult.NotFound;

            viewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
            return ServiceResult.OK;
        }
        
        public void SetRoleApplicationViewData(ViewDataDictionary viewData)
        {
            viewData["ButtonsPartialName"] = GetButtonsPartialString();
        }
        
        private string GetButtonsPartialString()
        {
            var role = userService.GetUserRole();
            if (role == UserRole.User)
                return "_ApplicationButtonsUser";
            else if (role == UserRole.Hr)
                return "_ApplicationButtonsHr";
            else
                return null;
        }
    }
}
