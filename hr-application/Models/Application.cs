using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hr_application.Models
{
    public enum ApplicationState
    {
        Pending,
        Rejected,
        Accepted
    }

    public class Application
    {
        public Guid Id { get; set; }
        public int Version { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ApplicationState State { get; set; }

        public string CvGuid { get; set; }

        public string CoverLetterGuid { get; set; }

        public Guid RelatedOfferId { get; set; }
        public JobOffer RelatedOffer { get; set; }
    }
}
