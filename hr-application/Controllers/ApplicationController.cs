using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Models;
using hr_application.ViewModels;
using Microsoft.AspNetCore.Http;
using hr_application.Services;

namespace hr_application.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly HrContext hrContext;
        private readonly ApplicationService applicationService;
        public ApplicationController(HrContext hrContext, ApplicationService applicationService)
        {
            this.hrContext = hrContext;
            this.applicationService = applicationService;
        }

        public IActionResult Index()
        {
            var applications = hrContext.Applications.ToList();
            var applicationViewModels = new List<ApplicationListItemViewModel>();
            foreach (var app in applications)
                applicationViewModels.Add(new ApplicationListItemViewModel(app));

            return View(applicationViewModels);
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
                return NotFound();

            if (!FillJobOfferViewdata(id.Value))
                return NotFound();

            var application = new ApplicationFormViewModel { RelatedOfferId = id.Value };
            return View(application);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationFormViewModel application)
        {
            var jobOffer = hrContext.JobOffers.Find(application.RelatedOfferId);
            if (jobOffer == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
                return View(application);
            }

            var applicationEntity = new Application
            {
                Email = application.Email,
                FirstName = application.FirstName,
                LastName = application.LastName,
                PhoneNumber = application.PhoneNumber,
                RelatedOfferId = application.RelatedOfferId,
                UserId = "0" //placeholder
            };

            hrContext.Applications.Add(applicationEntity);
            hrContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid ?id)
        {
            if (id == null)
                return NotFound();

            var application = hrContext.Applications.Find(id);
            if (application == null)
                return NotFound();

            if (!FillJobOfferViewdata(application.RelatedOfferId))
                return NotFound();
            
            return View(new ApplicationFormViewModel(application));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, ApplicationFormViewModel application)
        {
            if (ModelState.IsValid)
            {
                var foundApplication = hrContext.Applications.Find(id);
                if (foundApplication == null)
                    return NotFound();

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

                return RedirectToAction("Index");
            }
            
            if (!FillJobOfferViewdata(application.RelatedOfferId))
                return NotFound();

            return View(application);
        }

        public IActionResult Delete(Guid id)
        {
            var application = hrContext.Applications.Find(id);
            if (application != null)
            {
                hrContext.Applications.Remove(application);
                hrContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        private bool FillJobOfferViewdata(int id)
        {
            var jobOffer = hrContext.JobOffers.Find(id);
            if (jobOffer == null)
                return false;

            ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
            return true;
        }
    }
}
