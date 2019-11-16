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
    public class ApplicationController : Controller
    {
        private readonly HrContext hrContext;
        private readonly ApplicationService applicationService;
        private readonly IUserService userService;

        public ApplicationController(HrContext hrContext, ApplicationService applicationService, IUserService userService)
        {
            this.hrContext = hrContext;
            this.applicationService = applicationService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            var applications = applicationService.GetAllApplications();
            return View(applications);
        }

        public IActionResult MyApplications()
        {
            if (!userService.IsAuthenticated())
                return RedirectToAction(userService.GetRedirectToLoginUrl());
            
            var userId = userService.GetUserId();
            return View(applicationService.GetApplicaionsForUser(userId));
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
                return NotFound();

            if (!applicationService.FillJobOfferViewdata(id.Value, ViewData))
                return NotFound();

            if (!userService.IsAuthenticated())
                return RedirectToAction(userService.GetRedirectToLoginUrl());

            var application = new ApplicationFormViewModel { RelatedOfferId = id.Value };
            return View(application);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationFormViewModel application)
        {
            if (!userService.IsAuthenticated())
                return RedirectToAction(userService.GetRedirectToLoginUrl());

            var jobOffer = hrContext.JobOffers.Find(application.RelatedOfferId);
            if (jobOffer == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
                return View(application);
            }

            var userId = userService.GetUserId(); 
            applicationService.AddApplication(application, userId);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid ?id)
        {
            if (id == null)
                return NotFound();

            if (!userService.IsAuthenticated())
                return RedirectToAction(userService.GetRedirectToLoginUrl());

            var application = hrContext.Applications.Find(id);
            var userId = userService.GetUserId();
            if (application == null || application.UserId != userId)
                return NotFound();

            if (!applicationService.FillJobOfferViewdata(application.RelatedOfferId, ViewData))
                return NotFound();
            
            return View(new ApplicationFormViewModel(application));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, ApplicationFormViewModel application)
        {
            if (!userService.IsAuthenticated())
                return RedirectToAction(userService.GetRedirectToLoginUrl());

            if (ModelState.IsValid)
            {
                var userId = userService.GetUserId();
                if (applicationService.EditApplication(id, userId, application))
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
            if (!userService.IsAuthenticated())
                return RedirectToAction(userService.GetRedirectToLoginUrl());

            var userId = userService.GetUserId();
            if (applicationService.DeleteApplication(id, userId))
                return RedirectToAction("Index");
            else
                return NotFound();
        }
    }
}
