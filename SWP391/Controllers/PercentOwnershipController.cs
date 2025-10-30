using Microsoft.AspNetCore.Mvc;
using Services;
using Repositories.DTOs.PercentOwnership;
using Repositories.Entities;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PercentOwnershipController : ControllerBase
    {
        private readonly PercentOwnershipService _percentOwnershipService;

        public PercentOwnershipController(PercentOwnershipService percentOwnershipService)
        {
            _percentOwnershipService = percentOwnershipService;
        }

        // ✅ GET: api/percentownership
        [HttpGet]
        public async Task<IActionResult> GetAllPercentOwnerships()
        {
            var percentOwnerships = await _percentOwnershipService.GetAllPercentOwnershipsAsync();
            return Ok(percentOwnerships);
        }

        // ✅ GET: api/percentownership/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPercentOwnershipById(int id)
        {
            var percentOwnership = await _percentOwnershipService.GetPercentOwnershipByIdAsync(id);
            if (percentOwnership == null)
                return NotFound($"PercentOwnership with id {id} not found.");

            return Ok(percentOwnership);
        }

        // ✅ POST: api/percentownership
        [HttpPost]
        public async Task<IActionResult> CreatePercentOwnership([FromBody] PercentOwnershipCreateDto dto)
        {
            try
            {
                var createdPercentOwnership = await _percentOwnershipService.CreatePercentOwnershipAsync(dto);
                return CreatedAtAction(nameof(GetPercentOwnershipById), new { id = createdPercentOwnership.PercentOwnershipId }, createdPercentOwnership);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ PUT: api/percentownership/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePercentOwnership(int id, [FromBody] PercentOwnershipUpdateDto dto)
        {
            try
            {
                var updatedPercentOwnership = await _percentOwnershipService.UpdatePercentOwnershipAsync(id, dto);
                return Ok(updatedPercentOwnership);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // ✅ DELETE: api/percentownership/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePercentOwnership(int id)
        {
            var result = await _percentOwnershipService.DeletePercentOwnershipAsync(id);
            if (!result)
                return NotFound($"PercentOwnership with id {id} not found.");

            return Ok($"PercentOwnership with id {id} has been deleted.");
        }
    }
}
