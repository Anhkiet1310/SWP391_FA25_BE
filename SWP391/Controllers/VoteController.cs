using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.Vote;
using Services;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly VoteService _voteService;

        public VoteController(VoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVotes()
        {
            var votes = await _voteService.GetAllVotesAsync();
            if (votes == null || !votes.Any())
                return NotFound("No votes found.");

            return Ok(votes);
        }


        [HttpPost]
        public async Task<IActionResult> CreateVote([FromBody] VoteCreateDto dto)
        {
            var (success, message, vote) = await _voteService.CreateVoteAsync(dto);

            if (!success)
                return Conflict(new { Message = message });

            return Ok(new
            {
                vote!.VoteId,
                vote.UserId,
                vote.FormId,
                vote.Decision,
                vote.CreatedAt
            });
        }
    }
}
