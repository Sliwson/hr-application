using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_application.Services;
using hr_application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hr_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOfferApiController : ControllerBase
    {
        private readonly HrContext context;
        private readonly JobOfferService jobOfferService;
        private readonly IUserService userService;

        public JobOfferApiController(HrContext context, JobOfferService jobOfferService, IUserService userService)
        {
            this.context = context;
            this.jobOfferService = jobOfferService;
            this.userService = userService;
        }

        // GET: api/JobOfferApi
        [HttpGet]
        public IEnumerable<JobOfferListItemViewModel> Get(int count)
        {
            return jobOfferService.GetLastJobOffers(count);
        }
    }
}
