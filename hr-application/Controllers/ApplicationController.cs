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
        private readonly JobOfferService jobOfferService;
        private readonly IUserService userService;

        public ApplicationController(HrContext hrContext, ApplicationService applicationService, JobOfferService jobOfferService, IUserService userService)
        {
            this.hrContext = hrContext;
            this.applicationService = applicationService;
            this.jobOfferService = jobOfferService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            applicationService.SetRoleApplicationViewData(ViewData);

            var role = userService.GetUserRole();
            if (role == UserRole.Admin)
                return View(applicationService.GetAllApplications());
            else if (role == UserRole.Hr)
                return View(applicationService.GetHrUserApplications());
            else if (role == UserRole.User)
                return View(applicationService.GetUserApplications());
            else
                return RedirectToLogin();
        }

        public IActionResult Query([FromQuery(Name = "q")] string query)
        {
            if (userService.GetUserRole() != UserRole.Hr)
                return StatusCode(403);

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

            if (jobOfferService.IsJobOfferOutdated(id.Value))
                return View("Outdated");

            var application = new ApplicationFormViewModel { RelatedOfferId = id.Value };
            return View(application);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationFormViewModel application)
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

            await applicationService.AddApplication(application);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid ?id)
        {
            if (id == null)
                return NotFound();

            if (userService.GetUserRole() != UserRole.User)
                return StatusCode(403);

            var application = hrContext.Applications.Find(id);
            var userId = userService.GetUserId();

            if (application == null)
                return NotFound();
            if (application.UserId != userId)
                return StatusCode(403);
            if (application.State != ApplicationState.Pending)
                return StatusCode(422);

            if (applicationService.FillJobOfferViewdata(application.RelatedOfferId, ViewData) == ServiceResult.NotFound)
                return NotFound();
            
            return View(new ApplicationFormViewModel(application));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ApplicationFormViewModel application)
        {
            if (userService.GetUserRole() != UserRole.User)
                return RedirectToLogin();

            if (ModelState.IsValid)
            {
                var actionResult = await applicationService.EditApplication(id, application);
                if (actionResult == ServiceResult.SimultanousEdit)
                    ModelState.AddModelError(String.Empty, "Entry already edited");
                else
                    return ResolveServiceResult(actionResult);
            }
            
            if (applicationService.FillJobOfferViewdata(application.RelatedOfferId, ViewData) == ServiceResult.NotFound)
                return NotFound();

            return View(application);
        }

        public async Task<IActionResult> Details(Guid ?id)
        {
            if (userService.GetUserRole() == UserRole.Hr)
                return await HrDetails(id);

            return NotFound();
        }

        private async Task<IActionResult> HrDetails(Guid ?id)
        {
            if (userService.GetUserRole() != UserRole.Hr)
                return StatusCode(403);

            if (id == null)
                return NotFound();

            var application = hrContext.Applications.Find(id);
            if (application == null)
                return NotFound();

            if (applicationService.FillJobOfferViewdata(application.RelatedOfferId, ViewData) == ServiceResult.NotFound)
                return NotFound();

            var model = await applicationService.GetHrDetails(application);
            return View("HrDetails", model);
        }

        public IActionResult SetState(Guid id, [FromQuery(Name = "s")] int state)
        {
            if (userService.GetUserRole() != UserRole.Hr)
                return StatusCode(403);

            var actionResult = applicationService.ChangeApplicationState(id, state);
            return ResolveServiceResult(actionResult);
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
                case ServiceResult.ArgumentError:
                    return StatusCode(422);
                case ServiceResult.OK:
                default:
                    return RedirectToAction("Index");
            }
        }
    }
}
