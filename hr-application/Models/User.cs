using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Services;

namespace hr_application.Models
{
    public class User
    {
        public string Identifier { get; set; }
        public  UserRole Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
