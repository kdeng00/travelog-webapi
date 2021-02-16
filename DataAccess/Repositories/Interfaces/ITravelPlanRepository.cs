﻿using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITravelPlanRepository
    {
        Task CreateAsync(TravelPlanDto travelPlanDto, string userId); 
        Task<TravelPlan> GetAsync(Guid travelPlanId);
    }
}
