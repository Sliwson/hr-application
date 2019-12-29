using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Models;
using hr_application.ViewModels;
using hr_application.Services;

namespace hr_application.Controllers
{
    public class JobOfferController : Controller
    {
        private readonly HrContext hrContext;
        private readonly JobOfferService jobOfferService;
        private readonly ApplicationService applicationService;
        private readonly IUserService userService;

        public JobOfferController(HrContext hrContext, JobOfferService jobOfferService, ApplicationService applicationService, IUserService userService)
        {
            this.hrContext = hrContext;
            this.jobOfferService = jobOfferService;
            this.applicationService = applicationService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            jobOfferService.SetRoleJobOfferViewData(ViewData);

            if (userService.GetUserRole() == UserRole.Hr)
                return View(jobOfferService.GetUserJobOffers());
            else
                return View(jobOfferService.GetAllJobOffers());
        }

        public IActionResult Create()
        {
            if (userService.GetUserRole() != UserRole.Hr)
                return StatusCode(403);

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(JobOfferFormViewModel jobOffer)
        {
            if (userService.GetUserRole() != UserRole.Hr)
                return StatusCode(403);

            if (!ModelState.IsValid)
                return View(jobOffer);

            jobOfferService.AddJobOffer(jobOffer);
            return RedirectToAction("Index");
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var offer = hrContext.JobOffers.Find(id);
            if (offer == null)
                return NotFound();

            jobOfferService.SetRoleJobOfferViewData(ViewData);

            ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(offer);
            return View();
        }

        public IActionResult Edit(Guid ?id)
        {
            var edit = jobOfferService.GetEditViewModel(id);
            if (edit == null)
                return NotFound();
            else
                return View(edit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, JobOfferFormViewModel jobOffer)
        {
            if (ModelState.IsValid)
            {
                if (jobOfferService.EditOffer(id, jobOffer))
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError(String.Empty, "Entry already edited");
                    return View(jobOffer);
                }
            }

            return View(jobOffer);
        }

        public IActionResult Delete(Guid id)
        {
            var result = jobOfferService.DeleteJobOffer(id);
            if (result == JobOfferService.JobOfferDeleteResult.OK)
                return RedirectToAction("Index");
            else if (result == JobOfferService.JobOfferDeleteResult.NotFound)
                return NotFound();
            else if (result == JobOfferService.JobOfferDeleteResult.NotAuthorized)
                return StatusCode(403);
            else if (result == JobOfferService.JobOfferDeleteResult.ApplicationsPresent)
            {
                applicationService.FillJobOfferViewdata(id, ViewData);
                return View("JobOfferApplicationsError");
            }

            return RedirectToAction("Index");
        }
    }
}
