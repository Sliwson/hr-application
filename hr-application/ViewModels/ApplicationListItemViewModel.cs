using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Models;

namespace hr_application.ViewModels
{
    public class ApplicationListItemViewModel
    {
        public ApplicationListItemViewModel(Application application)
        {
            Id = application.Id;
            Email = application.Email;
            FirstName = application.FirstName;
            LastName = application.LastName;
            PhoneNumber = application.PhoneNumber;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
