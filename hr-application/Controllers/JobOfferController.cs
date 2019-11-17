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
        private readonly IUserService userService;

        public JobOfferController(HrContext hrContext, JobOfferService jobOfferService, IUserService userService)
        {
            this.hrContext = hrContext;
            this.jobOfferService = jobOfferService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
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
                    return View(jobOffer);
            }

            return View(jobOffer);
        }

        public IActionResult Delete(Guid id)
        {
            if (jobOfferService.DeleteJobOffer(id))
                return RedirectToAction("Index");
            else
                return NotFound();
        }
    }
}
