using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Models;

namespace hr_application.ViewModels
{
    public class JobOfferDetailsViewModel : JobOfferListItemViewModel
    {
        public JobOfferDetailsViewModel(JobOffer jobOffer)
            : base(jobOffer)
        {
            Description = jobOffer.Description;
        }

        public string Description { get; set; }
    }
}
