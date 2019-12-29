using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_application.Services
{
    public enum ServiceResult
    {
        OK,
        NotAuthorized,
        NotFound,
        ArgumentError,
        SimultanousEdit
    }
}
