using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.Form;
using Services;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly FormService _formService;

        public FormController(FormService formService)
        {
            _formService = formService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateForm([FromBody] FormCreateDto dto)
        {
            var form = await _formService.CreateFormAsync(dto);
            if (form == null)
                return NotFound($"Group with id {dto.GroupId} not found.");

            return CreatedAtAction(nameof(GetFormById), new { id = form.FormId }, form);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFormById(int id)
        {
            var form = await _formService.GetFormByIdAsync(id);
            if (form == null) return NotFound($"Form with id {id} not found.");

            return Ok(form);
        }

        [HttpGet("{id}/results")]
        public async Task<IActionResult> GetFormResults(int id)
        {
            var result = await _formService.GetFormResultsAsync(id);
            if (result == null)
                return NotFound($"Form with id {id} not found or has no votes.");

            return Ok(result);
        }
    }
}
