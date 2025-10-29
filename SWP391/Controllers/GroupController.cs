using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.DTOs.Group;
using Repositories.Entities;
using Services;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly GroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        // ✅ GET: api/group
        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }

        // ✅ GET: api/group/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null)
                return NotFound($"Group with id {id} not found");

            return Ok(group);
        }

        // ✅ POST: api/group
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreateDto dto)
        {
            try
            {
                var group = await _groupService.CreateGroupAsync(dto);
                return CreatedAtAction(nameof(GetGroupById), new { id = group.GroupId }, group);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Nếu có lỗi (vd: xe đã có nhóm)
            }
        }

        // ✅ PUT: api/group/{id}/update
        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] GroupUpdateDto dto)
        {
            try
            {
                var group = await _groupService.UpdateGroupAsync(id, dto);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Nếu không tìm thấy group hoặc lỗi khác
            }
        }

        // ✅ DELETE: api/group/{id}/delete
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var result = await _groupService.DeleteGroupAsync(id);
            if (!result)
                return NotFound($"Group with id {id} not found");

            return Ok($"Group with id {id} has been deleted");
        }
    }
}
