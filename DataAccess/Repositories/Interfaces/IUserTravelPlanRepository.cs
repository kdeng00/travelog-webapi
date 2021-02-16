﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserTravelPlanRepository
    {
        Task<IEnumerable<Guid>> GetTravelersForActivity(Guid travelPlanId);
    }
}
