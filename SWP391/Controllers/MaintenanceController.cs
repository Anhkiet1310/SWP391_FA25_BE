using Repositories.DTOs.Maintenance;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly MaintenanceService _maintenanceService;

        public MaintenanceController(MaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        // ✅ GET: /api/maintenance
        [HttpGet]
        public async Task<IActionResult> GetAllMaintenance()
        {
            var maintenances = await _maintenanceService.GetAllMaintenanceAsync();
            if (maintenances == null || !maintenances.Any())
                return NotFound("No maintenance records found.");

            return Ok(maintenances);
        }


        // ✅ POST: /api/maintenance
        [HttpPost]
        public async Task<IActionResult> CreateMaintenance([FromBody] MaintenanceCreateDto dto)
        {
            var maintenance = await _maintenanceService.CreateMaintenanceAsync(dto);
            if (maintenance == null)
                return BadRequest("Failed to create maintenance.");

            return CreatedAtAction(nameof(GetMaintenanceById), new { id = maintenance.MaintenanceId }, maintenance);
        }

        // ✅ GET: /api/maintenance/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaintenanceById(int id)
        {
            var maintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);
            if (maintenance == null)
                return NotFound($"Maintenance with id {id} not found.");

            return Ok(maintenance);
        }

        // ✅ DELETE: /api/maintenance/{id}/delete
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteMaintenance(int id)
        {
            var maintenance = await _maintenanceService.DeleteMaintenanceAsync(id);
            if (maintenance == null)
                return NotFound($"Maintenance with id {id} not found.");

            return Ok(new { Message = $"Maintenance with id {id} has been deleted." });
        }
    }
}
