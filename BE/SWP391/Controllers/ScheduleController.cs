using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.Schedule;
using Services;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;
        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            try
            {
                var schedules = await _scheduleService.GetAllSchedules();
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid schedule ID");
            }
            try
            {
                var schedule = await _scheduleService.GetScheduleById(id);
                if (schedule == null)
                    return NotFound(new { message = "Not found!" });

                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSchedulesByUserId(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID");
            }
            try
            {
                var schedules = await _scheduleService.GetSchedulesByUserId(userId);
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateSchedule(ScheduleCreateDto scheduleCreateDto)
        {
            if (scheduleCreateDto == null)
            {
                return BadRequest("Schedule data is required");
            }
            try
            {
                var createdSchedule = await _scheduleService.CreateSchedule(scheduleCreateDto);
                return Ok(createdSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, ScheduleUpdateDto scheduleUpdateDto)
        {
            if (id <= 0 || scheduleUpdateDto == null)
            {
                return BadRequest("Invalid input data");
            }
            try
            {
                var updatedSchedule = await _scheduleService.UpdateSchedule(id, scheduleUpdateDto);
                if (updatedSchedule == null)
                    return NotFound(new { message = "Not found!" });
                return Ok(updatedSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid schedule ID");
            }
            try
            {
                var result = await _scheduleService.DeleteSchedule(id);
                if (!result)
                {
                    return NotFound(new { message = "Not found!" });
                }
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}