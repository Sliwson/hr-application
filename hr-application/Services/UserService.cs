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
        IActionResult RedirectToLogin(Controller c);
        List<User> ListUsers();
        bool ChangeUserRole(string id, UserRole role);
        void TryAddCurrentUser();
        /* DEBUG CODE */
        void AuthenticateAs(UserRole role);
    }

    public class MockUserService : IUserService
    {
        public static List<User> _users = new List<User>
        {
            new User { Identifier = "test-user", Email = "test@test.pl", Role = UserRole.User },
            new User { Identifier = "test-hr", Email = "test@test.pl", Role = UserRole.Hr },
            new User { Identifier = "test-admin", Email = "test@test.pl", Role = UserRole.Admin }
        };

        public string GetUserId()
        {
            if (currentId < 0)
                return "";
            else
                return _users[currentId].Identifier;
        }

        public UserRole GetUserRole()
        {
            if (currentId < 0)
                return UserRole.NoAuth;
            else
                return _users[currentId].Role;
        }

        public bool IsAuthenticated()
        {
            return currentId != -1;
        }

        /* DEBUG CODE */
        public void AuthenticateAs(UserRole role)
        {
            if (role == UserRole.Admin)
                currentId = 2;
            else if (role == UserRole.Hr)
                currentId = 1;
            else if (role == UserRole.User)
                currentId = 0;
            else
                currentId = -1;
        }

        public List<User> ListUsers()
        {
            return new List<User>();
        }

        public bool ChangeUserRole(string id, UserRole role)
        {
            return true;
        }

        public void TryAddCurrentUser()
        {
        }

        public IActionResult RedirectToLogin(Controller c)
        {
            return c.RedirectToAction("Index", "Home"); 
        }

        private static int currentId = 0;
    }
}
