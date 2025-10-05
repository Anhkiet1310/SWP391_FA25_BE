using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.DTOs.Group;
using Repositories.Entities;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public GroupController(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/group
        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _context.Groups.Include(g => g.Car)// lấy thông tin xe liên kết (nếu cần)
                .ToListAsync();

            return Ok(groups);
        }

        // ✅ GET: api/group/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _context.Groups
                .Include(g => g.Car)
                .FirstOrDefaultAsync(g => g.GroupId == id);

            if (group == null)
                return NotFound($"Group with id {id} not found");

            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Group data is required");

            // Kiểm tra xe tồn tại
            var car = await _context.Cars.FindAsync(dto.CarId);
            if (car == null)
                return BadRequest($"Car with id {dto.CarId} does not exist");

            // ⚠️ Kiểm tra trùng (CarId đã có nhóm chưa)
            var existingGroup = await _context.Groups
                .FirstOrDefaultAsync(g => g.CarId == dto.CarId && g.DeleteAt == null);

            if (existingGroup != null)
                return Conflict($"Car with id {dto.CarId} already has a group '{existingGroup.GroupName}'.");

            var group = new Group
            {
                CarId = dto.CarId,
                GroupName = dto.GroupName,
                GroupImg = dto.GroupImg,
                CreatedAt = DateTime.UtcNow
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroupById), new { id = group.GroupId }, group);
        }

        // ✅ PUT: api/group/{id}/update
        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] GroupUpdateDto dto)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound($"Group with id {id} not found");

            group.GroupName = dto.GroupName ?? group.GroupName;
            group.GroupImg = dto.GroupImg ?? group.GroupImg;
            group.UpdatedAt = DateTime.UtcNow;

            _context.Groups.Update(group);
            await _context.SaveChangesAsync();

            return Ok(group);
        }

        // ✅ DELETE: api/group/{id}/delete
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound($"Group with id {id} not found");

            // Soft delete
            group.DeleteAt = DateTime.UtcNow;
            _context.Groups.Update(group);

            await _context.SaveChangesAsync();

            return Ok($"Group with id {id} has been deleted");
        }
    }

}
