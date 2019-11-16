using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Models;

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
        string GetRedirectToLoginAction();
        string GetRedirectToLoginController();
    }

    public class MockUserService : IUserService
    {
        public static List<User> _users = new List<User>
        {
            new User { Identifier = "test-user", Name = "Pan", Surname = "Pawel", Role = UserRole.User },
            new User { Identifier = "test-hr", Name = "Pan", Surname = "Hr", Role = UserRole.Hr },
            new User { Identifier = "test-admin", Name = "Pan", Surname = "Admin", Role = UserRole.Admin }
        };

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

        public string GetRedirectToLoginAction()
        {
            return "Index";
        }

        public string GetRedirectToLoginController()
        {
            return "Home";
        }
    }
}
