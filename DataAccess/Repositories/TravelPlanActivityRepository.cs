﻿using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Persistence;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TravelPlanActivityRepository : ITravelPlanActivityRepository
    {
        private readonly AppDbContext _dbContext;

        public TravelPlanActivityRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(TravelPlanActivityDto activityDto, Guid userId)
        {
            try
            {
                //check if TravelPlan exists
                var travelPlan = await _dbContext.TravelPlans.FindAsync(activityDto.TravelPlanId);

                if (travelPlan == null) throw new Exception("Travel Plan Not Found");

                var newActivity = new TravelPlanActivity
                {
                    Name = activityDto.Name,
                    StartTime = activityDto.StartTime,
                    EndTime = activityDto.EndTime,
                    Category = activityDto.Category,
                    Location = activityDto.Location,
                    HostId = userId,
                    TravelPlanId = activityDto.TravelPlanId
                };

                _dbContext.TravelPlanActivities.Add(newActivity);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccessful;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid activityId, Guid userId)
        {
            try
            {
                var activityToDelete = await _dbContext.TravelPlanActivities.FindAsync(activityId);

                if (activityToDelete == null)
                {
                    //log maybe?
                    return true;
                }
                if (activityToDelete.HostId != userId) throw new Exception("Dont have permission to delete");

                _dbContext.TravelPlanActivities.Remove(activityToDelete);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccessful;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EditAsync(TravelPlanActivityDto activityDto, Guid userId)
        {
            try
            {
                var activityToEdit = await _dbContext.TravelPlanActivities.FindAsync(activityDto.TravelPlanId);

                if (activityToEdit == null) throw new Exception("Activity not found");
                if (activityToEdit.HostId != userId) throw new Exception("Insufficient rights to edit activity");

                //map lib here
                activityToEdit.StartTime = activityDto.StartTime;
                activityToEdit.EndTime = activityDto.EndTime;
                activityToEdit.Location = activityDto.Location;
                activityToEdit.Category = activityDto.Category;

                if (!_dbContext.ChangeTracker.HasChanges()) return true;

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;
                return isSuccessful;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TravelPlanActivityDto> GetAsync(Guid activityId)
        {
            try
            {
                var activity = await _dbContext.TravelPlanActivities.FindAsync(activityId);

                if (activity == null) throw new Exception("Activity Not Found");

                return new TravelPlanActivityDto(activity);
            }
            catch
            {
                throw;
            }
        }
    }
}