using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_application.Services
{
    public enum UserRole
    {
        NoAuth,
        User,
        Hr,
        Admin
    }

    public interface IUserService
    {
        bool IsAuthenticated();
        string GetUserId();
        UserRole GetUserRole();
        string GetRedirectToLoginUrl();
    }

    public class MockUserService : IUserService
    {
        public string GetUserId()
        {
            return "test-user";
        }

        public UserRole GetUserRole()
        {
            return UserRole.Admin;
        }

        public bool IsAuthenticated()
        {
            return true;
        }

        public string GetRedirectToLoginUrl()
        {
            return "Index";
        }
    }
}
