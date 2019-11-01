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

            if (!FillJobOfferViewdata(id.Value))
                return NotFound();

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

        public IActionResult Edit(int ?id)
        {
            if (id == null)
                return NotFound();

            var application = Application._applications.Find(x => x.Id == id);
            if (application == null)
                return NotFound();

            if (!FillJobOfferViewdata(application.RelatedOfferId))
                return NotFound();
            
            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Application application)
        {
            if (id != application.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var foundApplication = Application._applications.Find(x => x.Id == id);
                if (foundApplication == null)
                    return NotFound();

                Application._applications[Application._applications.IndexOf(foundApplication)] = application;
                return RedirectToAction("Index");
            }

            if (!FillJobOfferViewdata(application.RelatedOfferId))
                return NotFound();

            return View(application);
        }

        public IActionResult Delete(int id)
        {
            var application = Application._applications.Find(x => x.Id == id);
            if (application != null)
            {
                Application._applications.Remove(application);    
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        private bool FillJobOfferViewdata(int id)
        {
            var jobOffer = JobOffer._jobOffers.Find(j => j.Id == id);
            if (jobOffer == null)
                return false;

            ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
            return true;
        }
    }
}
