using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Models;
using hr_application.ViewModels;
using Microsoft.AspNetCore.Http;
using hr_application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace hr_application.Controllers
{
    [Authorize]
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
            var applications = applicationService.GetAllApplications();
            return View(applications);
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
                return NotFound();

            if (!applicationService.FillJobOfferViewdata(id.Value, ViewData))
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

            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            applicationService.AddApplication(application, userId.Value);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid ?id)
        {
            if (id == null)
                return NotFound();

            var application = hrContext.Applications.Find(id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (application == null || application.UserId != userId.Value)
                return NotFound();

            if (!applicationService.FillJobOfferViewdata(application.RelatedOfferId, ViewData))
                return NotFound();
            
            return View(new ApplicationFormViewModel(application));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, ApplicationFormViewModel application)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier);
                if (applicationService.EditApplication(id, userId.Value, application))
                    return RedirectToAction("Index");
                else
                    return NotFound();
            }
            
            if (!applicationService.FillJobOfferViewdata(application.RelatedOfferId, ViewData))
                return NotFound();

            return View(application);
        }

        public IActionResult Delete(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (applicationService.DeleteApplication(id, userId.Value))
                return RedirectToAction("Index");
            else
                return NotFound();
        }
    }
}
