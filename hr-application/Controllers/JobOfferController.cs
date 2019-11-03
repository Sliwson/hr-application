using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Models;
using hr_application.ViewModels;

namespace hr_application.Controllers
{
    public class JobOfferController : Controller
    {
        private readonly HrContext hrContext;

        public JobOfferController(HrContext hrContext)
        {
            this.hrContext = hrContext;
        }

        public IActionResult Index()
        {
            List<JobOfferListItemViewModel> displayList = new List<JobOfferListItemViewModel>();
            var jobOffers = hrContext.JobOffers.ToList();
            foreach (var item in jobOffers)
                displayList.Add(new JobOfferListItemViewModel(item));

            return View(displayList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(JobOffer jobOffer)
        {
            if (!ModelState.IsValid)
            {
                return View(jobOffer);
            }

            hrContext.Add(jobOffer);
            hrContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var offer = hrContext.JobOffers.Find(id);

            if (offer == null)
                return NotFound();

            ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(offer);
            return View();
        }

        public IActionResult Edit(int ?id)
        {
            if (id == null)
                return NotFound();

            var offer = hrContext.JobOffers.Find(id);
            if (offer == null)
                return NotFound();

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, JobOffer jobOffer)
        {
            if (id != jobOffer.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var foundOffer = hrContext.JobOffers.Find(id);
                if (foundOffer == null)
                    return NotFound();

                hrContext.Entry(foundOffer).CurrentValues.SetValues(jobOffer);
                hrContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(jobOffer);
        }

        public IActionResult Delete(int id)
        {
            var offer = hrContext.JobOffers.Find(id);
            if (offer != null)
            {
                hrContext.Remove(offer);
                hrContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
