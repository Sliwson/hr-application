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

        public IActionResult Query([FromQuery(Name = "q")] string query)
        {
            if (userService.GetUserRole() != UserRole.Hr)
                return StatusCode(403);

            if (query == null)
                query = "";

            return View("Index", applicationService.GetHrUserApplicationsFiltered(query));
        }

        public IActionResult Create(Guid? id)
        {
            if (id == null)
                return NotFound();

            if (applicationService.FillJobOfferViewdata(id.Value, ViewData) == ServiceResult.NotFound)
                return NotFound();

            if (!userService.IsAuthenticated())
                return RedirectToLogin();

            var application = new ApplicationFormViewModel { RelatedOfferId = id.Value };
            return View(application);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationFormViewModel application)
        {
            if (userService.GetUserRole() != UserRole.User)
                return RedirectToLogin();

            var jobOffer = hrContext.JobOffers.Find(application.RelatedOfferId);
            if (jobOffer == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
                return View(application);
            }

            applicationService.AddApplication(application);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid ?id)
        {
            if (id == null)
                return NotFound();

            if (userService.GetUserRole() != UserRole.User)
                return RedirectToLogin();

            var application = hrContext.Applications.Find(id);
            var userId = userService.GetUserId();

            if (application == null)
                return NotFound();
            if (application.UserId != userId)
                return StatusCode(403);

            if (applicationService.FillJobOfferViewdata(application.RelatedOfferId, ViewData) == ServiceResult.NotFound)
                return NotFound();
            
            return View(new ApplicationFormViewModel(application));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, ApplicationFormViewModel application)
        {
            if (userService.GetUserRole() != UserRole.User)
                return RedirectToLogin();

            if (ModelState.IsValid)
            {
                var actionResult = applicationService.EditApplication(id, application);
                return ResolveServiceResult(actionResult);
            }
            
            if (applicationService.FillJobOfferViewdata(application.RelatedOfferId, ViewData) == ServiceResult.NotFound)
                return NotFound();

            return View(application);
        }

        public IActionResult Delete(Guid id)
        {
            if (!userService.IsAuthenticated())
                return RedirectToLogin();

            var actionResult = applicationService.DeleteApplication(id);
            return ResolveServiceResult(actionResult);
        }

        private IActionResult RedirectToLogin()
        {
            return RedirectToAction(userService.GetRedirectToLoginAction(), userService.GetRedirectToLoginController());
        }

        private IActionResult ResolveServiceResult(ServiceResult result)
        {
            switch (result)
            {
                case ServiceResult.NotFound:
                    return NotFound();
                case ServiceResult.NotAuthorized:
                    return StatusCode(403);
                case ServiceResult.OK:
                default:
                    return RedirectToAction("Index");
            }
        }
    }
}
