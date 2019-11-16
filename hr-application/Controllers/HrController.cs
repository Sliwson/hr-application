using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Services;
using Microsoft.AspNetCore.Mvc;

namespace hr_application.Controllers
{
    public class HrController : Controller
    {
        private IUserService userService;

        public HrController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            if (userService.GetUserRole() != UserRole.Hr)
                return StatusCode(403);

            return View();
        }

        /* DEBUG CODE */
        public IActionResult Login()
        {
            userService.AuthenticateAs(UserRole.Hr);
            return RedirectToAction("Index");
        }
        
        public IActionResult Logout()
        {
            userService.AuthenticateAs(UserRole.NoAuth);
            return RedirectToAction("Index");
        }
    }
}