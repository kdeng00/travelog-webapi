﻿using DataAccess.CustomExceptions;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TravelogApi.Controllers
{
    public class TravelPlanActivityController : Controller
    {
        private readonly ITravelPlanActivityRepository _activityRepository;

        public TravelPlanActivityController(ITravelPlanActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TravelPlanActivityDto activityDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var newActivity = await _activityRepository.CreateAsync(activityDto, new Guid(userId));

                return Ok(newActivity);
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] TravelPlanActivityDto activityDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var editedActivityDto = await _activityRepository.EditAsync(activityDto, new Guid(userId));

                return Ok(editedActivityDto);
            }
            catch (InsufficientRightsException insufRights)
            {
                return BadRequest(new
                {
                    message = insufRights.Message
                });
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var isSuccessful = await _activityRepository.DeleteAsync(new Guid(id), new Guid(userId));

                if (!isSuccessful) return StatusCode(500);

                return Ok();
            }
            catch (InsufficientRightsException insufRights)
            {
                return BadRequest(new
                {
                    message = insufRights.Message
                });
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details([FromQuery] string id)
        {
            var activityDto = await _activityRepository.GetAsync(new Guid(id));

            return Ok(activityDto);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string id)
        {
            try
            {
                var lstActivityDto = await _activityRepository.ListAsync(new Guid(id));

                return Ok(lstActivityDto);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
