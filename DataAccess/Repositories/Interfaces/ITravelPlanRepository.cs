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
        Task<TravelPlanDto> CreateAsync(TravelPlanDto travelPlanDto, Guid userId);
        Task<TravelPlanDto> EditAsync(TravelPlanDto travelPlanDto, Guid userId);
        Task<bool> AddTravelerAsync(Guid travelPlanId, Guid loggedInUserId, Guid userId);
        Task<bool> DeleteAsync(Guid travelPlanId, Guid userId);
        Task<TravelPlanDto> GetAsync(Guid travelPlanId);
        Task<List<TravelPlanDto>> ListAsync(Guid userId);
    }
}
