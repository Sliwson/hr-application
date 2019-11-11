using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_application.Services
{
    public class ApplicationService
    {
        private readonly HrContext hrContext;

        public ApplicationService(HrContext context)
        {
            hrContext = context;
        }
    }
}
