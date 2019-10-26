using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Models;

namespace hr_application.Controllers
{
    public class JobOfferController : Controller
    {
        public IActionResult Index()
        {
            return View(JobOffer._jobOffers);
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