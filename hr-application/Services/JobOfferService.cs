using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Models;
using hr_application.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace hr_application.Services
{
    public class JobOfferService
    {
        private readonly HrContext hrContext;
        private readonly ApplicationService applicationService;
        private readonly IUserService userService;
    
        public enum JobOfferDeleteResult 
        {
            OK,
            NotFound,
            NotAuthorized,
            ApplicationsPresent
        }

        public JobOfferService(HrContext hrContext, ApplicationService applicationService, IUserService userService)
        {
            this.hrContext = hrContext;
            this.applicationService = applicationService;
            this.userService = userService;
        }

        public List<JobOfferListItemViewModel> GetAllJobOffers()
        {
            List<JobOfferListItemViewModel> displayList = new List<JobOfferListItemViewModel>();
            var jobOffers = hrContext.JobOffers.ToList();
            foreach (var item in jobOffers)
                displayList.Add(new JobOfferListItemViewModel(item));

            return displayList;
        }

        public List<JobOfferListItemViewModel> GetLastJobOffers(int count)
        {
            List<JobOfferListItemViewModel> displayList = new List<JobOfferListItemViewModel>();
            var jobOffers = hrContext.JobOffers.ToList().TakeLast(10);
            foreach (var item in jobOffers)
                displayList.Add(new JobOfferListItemViewModel(item));

            return displayList;
        }

        public List<JobOfferListItemViewModel> GetUserJobOffers()
        {
            if (userService.GetUserRole() != UserRole.Hr)
                return null;

            var userId = userService.GetUserId();
            List<JobOfferListItemViewModel> displayList = new List<JobOfferListItemViewModel>();
            var jobOffers = hrContext.JobOffers.Where(j => j.UserId == userId).ToList();
            foreach (var item in jobOffers)
                displayList.Add(new JobOfferListItemViewModel(item));

            return displayList;
        }

        public bool AddJobOffer(JobOfferFormViewModel jobOffer)
        {
            if (userService.GetUserRole() != UserRole.Hr)
                return false;

            var jobOfferModel = new JobOffer
            {
                Version = 0,
                Description = jobOffer.Description,
                ExpirationDate = jobOffer.ExpirationDate,
                JobTitle = jobOffer.JobTitle,
                MinimumSalary = jobOffer.MinimumSalary,
                MaximumSalary = jobOffer.MaximumSalary,
                Location = jobOffer.Location,
                UserId = userService.GetUserId()
            };

            hrContext.Add(jobOfferModel);
            hrContext.SaveChanges();
            return true;
        }

        public JobOfferFormViewModel GetEditViewModel(Guid? id)
        {
            if (id == null)
                return null;

            var offer = hrContext.JobOffers.Find(id);
            if (offer == null)
                return null;

            if (userService.GetUserId() != offer.UserId)
                return null;

            var editOffer = new JobOfferFormViewModel
            {
                Version = offer.Version,
                Description = offer.Description,
                JobTitle = offer.JobTitle,
                MaximumSalary = offer.MaximumSalary,
                MinimumSalary = offer.MinimumSalary,
                Location = offer.Location,
                ExpirationDate = offer.ExpirationDate
            };

            return editOffer;
        }

        public bool EditOffer(Guid id, JobOfferFormViewModel jobOffer)
        {
            var foundOffer = hrContext.JobOffers.Find(id);
            if (foundOffer == null)
                return false;

            if (userService.GetUserId() != foundOffer.UserId)
                return false;

            if (jobOffer.Version != foundOffer.Version)
                return false;

            var editModel = new JobOffer
            {
                Id = foundOffer.Id,
                Version = foundOffer.Version + 1,
                Description = jobOffer.Description,
                JobTitle = jobOffer.JobTitle,
                MinimumSalary = jobOffer.MinimumSalary,
                MaximumSalary = jobOffer.MaximumSalary,
                Location = jobOffer.Location,
                ExpirationDate = jobOffer.ExpirationDate,
                Applications = foundOffer.Applications,
                UserId = foundOffer.UserId
            };

            hrContext.Entry(foundOffer).CurrentValues.SetValues(editModel);
            hrContext.SaveChanges();

            return true;
        }

        public JobOfferDeleteResult DeleteJobOffer(Guid id)
        {
            var offer = hrContext.JobOffers.Find(id);
            if (offer == null)
                return JobOfferDeleteResult.OK;

            if (userService.GetUserId() != offer.UserId)
                return JobOfferDeleteResult.NotAuthorized;

            if (applicationService.GetApplicationsForJobOffer(id).Count > 0)
                return JobOfferDeleteResult.ApplicationsPresent;
            
            hrContext.Remove(offer);
            hrContext.SaveChanges();

            return JobOfferDeleteResult.OK; 
        }

        public void SetRoleJobOfferViewData(ViewDataDictionary viewData)
        {
            viewData["BeforePartialName"] = GetBeforePartialString();
            viewData["ButtonsPartialName"] = GetButtonsPartialString();
        }

        private string GetBeforePartialString()
        {
            if (userService.GetUserRole() == UserRole.Hr)
                return "_JobOfferIndexAddButton";
            else
                return null;
        }
        
        private string GetButtonsPartialString()
        {
            var role = userService.GetUserRole();
            if (role == UserRole.User)
                return "_JobOfferButtonsUser";
            else if (role == UserRole.Hr)
                return "_JobOfferButtonsHr";
            else
                return null;
        }
        
        public bool IsJobOfferOutdated(Guid id)
        {
            var jobOffer = hrContext.JobOffers.Find(id);
            if (jobOffer == null)
                return true;

            return jobOffer.ExpirationDate < DateTime.Now;
        }
    }
}
