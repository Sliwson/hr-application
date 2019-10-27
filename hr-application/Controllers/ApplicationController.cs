using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Models;
using hr_application.ViewModels;

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
            return View();
        }
    }
}
