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
            var role = userService.GetUserRole();
            if (role == UserRole.Admin)
                return View(applicationService.GetAllApplications());
            else if (role == UserRole.Hr)
                return View(applicationService.GetHrUserApplications());
            else if (role == UserRole.User)
                return View(applicationService.GetUserApplications());
            else
                return NotFound();
        }

        public IActionResult Create(Guid? id)
        {
            if (id == null)
                return NotFound();

            if (!applicationService.FillJobOfferViewdata(id.Value, ViewData))
                return NotFound();

            if (!userService.IsAuthenticated())
                return RedirectToLogin();

            var application = new ApplicationFormViewModel { RelatedOfferId = id.Value };
            return View(application);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationFormViewModel application)
        {
            if (!userService.IsAuthenticated())
                return RedirectToLogin();

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
                return RedirectToLogin();

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
                return RedirectToLogin();

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
                return RedirectToLogin();

            var userId = userService.GetUserId();
            if (applicationService.DeleteApplication(id, userId))
                return RedirectToAction("Index");
            else
                return NotFound();
        }

        private IActionResult RedirectToLogin()
        {
            return RedirectToAction(userService.GetRedirectToLoginAction(), userService.GetRedirectToLoginController());
        }
    }
}
