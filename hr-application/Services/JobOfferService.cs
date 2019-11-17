using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Models;
using hr_application.ViewModels;

namespace hr_application.Services
{
    public class JobOfferService
    {
        private readonly HrContext hrContext;
        private readonly IUserService userService;
    
        public JobOfferService(HrContext hrContext, IUserService userService)
        {
            this.hrContext = hrContext;
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

            var editModel = new JobOffer
            {
                Id = foundOffer.Id,
                Description = jobOffer.Description,
                JobTitle = jobOffer.JobTitle,
                MinimumSalary = jobOffer.MinimumSalary,
                MaximumSalary = jobOffer.MaximumSalary,
                Location = jobOffer.Location,
                ExpirationDate = jobOffer.ExpirationDate,
                Applications = foundOffer.Applications
            };

            hrContext.Entry(foundOffer).CurrentValues.SetValues(editModel);
            hrContext.SaveChanges();

            return true;
        }

        public bool DeleteJobOffer(Guid id)
        {
            var offer = hrContext.JobOffers.Find(id);
            if (offer == null)
                return false;

            if (userService.GetUserId() != offer.UserId)
                return false;
            
            hrContext.Remove(offer);
            hrContext.SaveChanges();

            return true; 
        }
    }
}
