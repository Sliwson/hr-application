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
        public IActionResult Index()
        {
            List<JobOfferListItemViewModel> displayList = new List<JobOfferListItemViewModel>();
            foreach (var item in JobOffer._jobOffers)
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

            JobOffer._jobOffers.Add(jobOffer);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var offer = JobOffer._jobOffers.FirstOrDefault(x => x.Id == id);

            if (offer == null)
                return NotFound();

            ViewData["JobOfferDetails"] = new JobOfferDetailsViewModel(offer);
            return View();
        }

        public IActionResult Edit(int ?id)
        {
            if (id == null)
                return NotFound();

            var offer = JobOffer._jobOffers.Find(x => x.Id == id);
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
                var foundOffer = JobOffer._jobOffers.Find(x => x.Id == id);
                if (foundOffer == null)
                    return NotFound();

                JobOffer._jobOffers[JobOffer._jobOffers.IndexOf(foundOffer)] = jobOffer;
                return RedirectToAction("Index");
            }

            return View(jobOffer);
        }

        public IActionResult Delete(int id)
        {
            var offer = JobOffer._jobOffers.Find(x => x.Id == id);
            if (offer != null)
            {
                JobOffer._jobOffers.Remove(offer);
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
