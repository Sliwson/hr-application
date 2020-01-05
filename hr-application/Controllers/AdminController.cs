using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_application.Services;
using hr_application.Models;
using hr_application.ViewModels;

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

            var list = GetViewModelUsers(userService.ListUsers());
            return View(list);
        }

        public IActionResult SetRole(string id, UserRole role)
        {
            if (userService.GetUserRole() != UserRole.Admin)
                return StatusCode(403);

            if (userService.ChangeUserRole(id, role))
                return RedirectToAction("Index");
            else return StatusCode(422);
        }

        /* DEBUG CODE */
#if DEBUG
        public IActionResult Login()
        {
            userService.AuthenticateAs(UserRole.Admin);
            return RedirectToAction("Index");
        }
#endif
        private List<UserViewModel> GetViewModelUsers(List<User> users)
        {
            var viewModelUsers = new List<UserViewModel>();
            foreach (var user in users)
                viewModelUsers.Add(new UserViewModel
                {
                    Email = user.Email,
                    Identifier = user.Identifier,
                    Role = user.Role
                });

            return viewModelUsers;
        }
    }
}