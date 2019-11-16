using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Services;

namespace hr_application.Controllers
{
    public class AdminController : Controller
    {
        private IUserService userService;

        public AdminController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            if (userService.GetUserRole() != UserRole.Admin)
                return StatusCode(403);

            return View();
        }

        /* DEBUG CODE */
        public IActionResult Login()
        {
            userService.AuthenticateAs(UserRole.Admin);
            return RedirectToAction("Index");
        }
        
        public IActionResult Logout()
        {
            userService.AuthenticateAs(UserRole.NoAuth);
            return RedirectToAction("Index");
        }
    }
}