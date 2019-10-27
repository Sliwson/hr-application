using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Models;
using hr_application.ViewModels;
using Microsoft.AspNetCore.Http;

namespace hr_application.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult Index()
        {
            return View(Application._applications);
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
                return NotFound();

            var jobOffer = JobOffer._jobOffers.Find(j => j.Id == id);
            if (jobOffer == null)
                return NotFound();

            ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);

            var application = new Application { RelatedOfferId = id.Value };
            return View(application);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Application application)
        {
            var jobOffer = JobOffer._jobOffers.Find(j => j.Id == application.RelatedOfferId);
            if (jobOffer == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
                return View(application);
            }

            Application._applications.Add(application);
            jobOffer.Applications.Add(application);

            return RedirectToAction("Index");
        }

    }
}
