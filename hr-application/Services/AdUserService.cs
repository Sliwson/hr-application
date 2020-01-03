using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using hr_application.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace hr_application.Services
{
    public class AdUserService : IUserService
    {
        private IHttpContextAccessor context;
        private readonly HrContext hrContext;

        public AdUserService(IHttpContextAccessor context, HrContext dbContext)
        {
            this.context = context;
            hrContext = dbContext;
        }

        public IActionResult RedirectToLogin(Controller c)
        {
            return c.RedirectToAction("SignIn", "Account", new { area = "AzureADB2C" });
        }

        public string GetUserId()
        {
            return context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        private string GetUserEmail()
        {
            var claimString = "emails";
            return context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == claimString).Value;
        }

        public UserRole GetUserRole()
        {
            if (!IsAuthenticated())
                return UserRole.NoAuth;

            var user = hrContext.Users.Where(u => u.Identifier == GetUserId()).FirstOrDefault();
            if (user == null)
                return UserRole.NoAuth;

            return user.Role;
        }

        public bool IsAuthenticated()
        {
            return context.HttpContext.User.Identity.IsAuthenticated;
        }

        public void AuthenticateAs(UserRole role)
        {
            return;
        }

        public List<User> ListUsers()
        {
            return hrContext.Users.Where(u => u.Role == UserRole.User).ToList(); 
        }

        public bool ChangeUserRole(string id, UserRole role)
        {
            var user = hrContext.Users.Where(u => u.Identifier == id).FirstOrDefault();
            if (user == null)
                return false;

            user.Role = role;
            hrContext.SaveChanges();
            return true;
        }

        public void TryAddCurrentUser()
        {
            if (!IsAuthenticated())
                return;

            var id = GetUserId();
            if (hrContext.Users.FirstOrDefault(u => u.Identifier == id) != null)
                return;

            var email = GetUserEmail();
            var user = new User
            {
                Email = email,
                Identifier = id,
                Role = UserRole.User
            };

            hrContext.Users.Add(user);
            hrContext.SaveChanges();
        }
    }
}
