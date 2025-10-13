using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class VoteRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public VoteRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        public async Task<Vote?> GetExistingVoteAsync(int userId, int formId)
        {
            return await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.FormId == formId && v.DeleteAt == null);
        }

        public async Task<Vote> AddVoteAsync(Vote vote)
        {
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
            return vote;
        }
    }
}
