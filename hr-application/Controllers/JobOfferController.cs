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
                displayList.Add(new JobOfferListItemViewModel(item.Id, item.JobTitle, item.MinimumSalary, item.MaximumSalary, item.Location, item.ExpirationDate));

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
    }
}