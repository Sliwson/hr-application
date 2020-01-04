using hr_application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_application.ViewModels
{
    public class UserViewModel
    {
        public string Identifier { get; set; }
        public  UserRole Role { get; set; }
        public string Email { get; set; }
    }
}
