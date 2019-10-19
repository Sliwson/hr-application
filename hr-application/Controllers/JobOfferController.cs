﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Models;

namespace hr_application.Controllers
{
    public class JobOfferController : Controller
    {
        public IActionResult List()
        {
            return View(JobOffer._jobOffers);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}