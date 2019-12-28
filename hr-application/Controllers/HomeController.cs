using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Models;
using hr_application.Services;

namespace hr_application.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Index()
        {
            ViewData["Role"] = userService.GetUserRole();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

#if DEBUG
        public IActionResult Login()
        {
            userService.AuthenticateAs(UserRole.User);
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            userService.AuthenticateAs(UserRole.NoAuth);
            return RedirectToAction("Index");
        }
#endif
    }
}
