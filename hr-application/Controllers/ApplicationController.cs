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
        private readonly HrContext hrContext;

        public ApplicationController(HrContext hrContext)
        {
            this.hrContext = hrContext;
        }

        public IActionResult Index()
        {
            return View(hrContext.Applications.ToList());
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
            var jobOffer = hrContext.JobOffers.Find(application.RelatedOfferId);
            if (jobOffer == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
                return View(application);
            }

            hrContext.Applications.Add(application);
            hrContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int ?id)
        {
            if (id == null)
                return NotFound();

            var application = hrContext.Applications.Find(id);
            if (application == null)
                return NotFound();

            if (!FillJobOfferViewdata(application))
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
                var foundApplication = hrContext.Applications.Find(id);
                if (foundApplication == null)
                    return NotFound();

                hrContext.Entry(foundApplication).CurrentValues.SetValues(application);
                return RedirectToAction("Index");
            }

            if (!FillJobOfferViewdata(application))
                return NotFound();

            return View(application);
        }

        public IActionResult Delete(int id)
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

        private bool FillJobOfferViewdata(Application application)
        {
            var jobOffer = application.RelatedOffer;
            if (jobOffer == null)
                return false;

            ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(jobOffer);
            return true;
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
