using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace hr_application.Services
{
    public class AdUserService : IUserService
    {
        private IHttpContextAccessor context;
        private static UserRole tmpRole = UserRole.User;

        public AdUserService(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public void AuthenticateAs(UserRole role)
        {
            tmpRole = role;
        }

        public string GetRedirectToLoginAction()
        {
            return "SignIn";
        }

        public string GetRedirectToLoginController()
        {
            return "Account";
        }

        public string GetUserId()
        {
            return context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
        }

        public UserRole GetUserRole()
        {
            return tmpRole;
        }

        public bool IsAuthenticated()
        {
            return context.HttpContext.User != null;
        }
    }
}
