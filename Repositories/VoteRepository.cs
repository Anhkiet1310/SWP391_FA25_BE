using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.DTOs.Form;
using Repositories.DTOs.User;
using Repositories.DTOs.Vote;
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

        public async Task<IEnumerable<VoteDto>> GetAllVotesAsync()
        {
            var votes = await _context.Votes
                .Include(v => v.User)  // Bao gồm thông tin User
                .Include(v => v.Form)  // Bao gồm thông tin Form
                .Where(v => v.DeleteAt == null)  // Lọc các bản ghi không bị xóa
                .ToListAsync();

            // Mapping thủ công từ Vote entity sang VoteDto
            var voteDtos = votes.Select(v => new VoteDto
            {
                VoteId = v.VoteId,
                UserId = v.UserId,
                FormId = v.FormId,
                Decision = v.Decision,
                User = new UserResponseDto  // Mapping thủ công User
                {
                    UserId = v.User.UserId,
                    UserName = v.User.UserName,
                    FullName = v.User.FullName,
                    Email = v.User.Email,
                    Balance = v.User.Balance,
                    PhoneNumber = v.User.PhoneNumber,
                    Gender = v.User.Gender,
                    Dob = v.User.Dob,
                    CCCDFront = v.User.CCCDFront,
                    CCCDBack = v.User.CCCDBack,
                    Role = v.User.Role
                },
                Form = new FormCreateDto  // Mapping thủ công Form
                {
                    GroupId = v.Form.GroupId,
                    FormTitle = v.Form.FormTitle,
                    StartDate = v.Form.StartDate,
                    EndDate = v.Form.EndDate,
                }
            }).ToList();

            return voteDtos;
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
