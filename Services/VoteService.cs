using Repositories;
using Repositories.DBContext;
using Repositories.DTOs.Vote;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class VoteService
    {
        private readonly VoteRepository _voteRepo;
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public VoteService(VoteRepository voteRepo, Co_ownershipAndCost_sharingDbContext context)
        {
            _voteRepo = voteRepo;
            _context = context;
        }

        public async Task<(bool Success, string Message, Vote? Vote)> CreateVoteAsync(VoteCreateDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return (false, $"User with id {dto.UserId} not found.", null);

            var form = await _context.Forms.FindAsync(dto.FormId);
            if (form == null)
                return (false, $"Form with id {dto.FormId} not found.", null);

            var existing = await _voteRepo.GetExistingVoteAsync(dto.UserId, dto.FormId);
            if (existing != null)
                return (false, $"User {dto.UserId} has already voted for form {dto.FormId}.", null);

            var vote = new Vote
            {
                UserId = dto.UserId,
                FormId = dto.FormId,
                Decision = dto.Decision,
                CreatedAt = DateTime.UtcNow
            };

            await _voteRepo.AddVoteAsync(vote);
            return (true, "Vote added successfully.", vote);
        }
    }
}
