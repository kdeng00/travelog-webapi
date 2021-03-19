﻿using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TravelPlanRepository : ITravelPlanRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public TravelPlanRepository(AppDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public async Task<bool> AddTravelerAsync(Guid travelPlanId, Guid userToAddId)
        {
            try
            {
                //check if travelplan exists
                var travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);
                if (travelPlan == null) throw new Exception("Travel Plan Not Found");


                //check if user exists
                var userExists = await _userRepository.DoesUserExistsAsync(userToAddId);
                if (!userExists) throw new Exception("Invalid User Id");

                var newUserTravelPlan = new UserTravelPlan
                {
                    UserId = userToAddId,
                    TravelPlanId = travelPlanId
                };

                //could throw exception if traveler is already added to travel plan bc of the composite key constraint
                _dbContext.UserTravelPlans.Add(newUserTravelPlan);
                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccessful;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TravelPlanDto> CreateAsync(TravelPlanDto travelPlanDto, Guid userId)
        {
            try
            {
                //map here
                var newTravelPlan = new TravelPlan
                {
                    Name = travelPlanDto.Name,
                    Description = travelPlanDto.Description,
                    StartDate = travelPlanDto.StartDate,
                    EndDate = travelPlanDto.EndDate,
                    CreatedById = userId
                };

                await _dbContext.TravelPlans.AddAsync(newTravelPlan);

                //add to jxn table
                var traveler = new UserTravelPlan
                {
                    TravelPlan = newTravelPlan,
                    UserId = userId
                };

                await _dbContext.UserTravelPlans.AddAsync(traveler);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                if (isSuccessful)
                {
                    return new TravelPlanDto(newTravelPlan);
                }
                throw new Exception("Problem Editing Travel Plan");
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid travelPlanId, Guid userId)
        {
            try
            {
                var travelPlanToDelete = await _dbContext.TravelPlans.FindAsync(travelPlanId);

                if (travelPlanToDelete == null) return true;

                if (travelPlanToDelete.CreatedById != userId) throw new Exception("Don't have permission to delete");

                //let EF core cascade delete and delete relations with dependent tables via collection nav properties
                _dbContext.Remove(travelPlanToDelete);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccessful;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TravelPlanDto> EditAsync(TravelPlanDto travelPlanDto, Guid userId)
        {
            try
            {
                var travelPlanToEdit = await _dbContext.TravelPlans.FindAsync(travelPlanDto.Id);
                if (travelPlanToEdit == null) throw new Exception("Travel Plan Not Found");

                if (travelPlanToEdit.CreatedById != userId) throw new Exception("Insufficient rights to edit Travel Plan");

                //map here
                travelPlanToEdit.TravelPlanId = travelPlanDto.Id;
                travelPlanToEdit.Name = travelPlanDto.Name;
                travelPlanToEdit.StartDate = travelPlanDto.StartDate;
                travelPlanToEdit.EndDate = travelPlanDto.EndDate;
                travelPlanToEdit.Description = travelPlanDto.Description;

                if (!_dbContext.ChangeTracker.HasChanges()) return travelPlanDto;

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                if (isSuccessful)
                {
                    return new TravelPlanDto(travelPlanToEdit);
                }
                throw new Exception("Problem Editing Travel Plan");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TravelPlanDto> GetAsync(Guid travelPlanId)
        {
            try
            {
                var travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);

                if (travelPlan == null) throw new Exception("Travel Plan not found");

                return new TravelPlanDto(travelPlan);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TravelPlanDto>> ListAsync(Guid userId)
        {
            try
            {
                var travelPlans = _dbContext.TravelPlans.Where((tp) => tp.CreatedById == userId).ToList();

                var lstTravelPlanDto = travelPlans.Select((tp) => new TravelPlanDto(tp)).ToList();

                return lstTravelPlanDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}