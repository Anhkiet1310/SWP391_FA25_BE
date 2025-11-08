using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.DBContext;
using Repositories.DTOs.Form;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FormService
    {
        private readonly FormRepository _formRepo;
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public FormService(FormRepository formRepo, Co_ownershipAndCost_sharingDbContext context)
        {
            _formRepo = formRepo;
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllFormsAsync()
        {
            var forms = await _context.Forms
                .Select(f => new
                {
                    f.FormId,
                    f.FormTitle,
                    f.StartDate,
                    f.EndDate,
                    f.GroupId
                })
                .ToListAsync();

            return forms;
        }


        public async Task<Form?> CreateFormAsync(FormCreateDto dto)
        {
            var group = await _context.Groups.FindAsync(dto.GroupId);
            if (group == null)
                return null;

            var form = new Form
            {
                GroupId = dto.GroupId,
                FormTitle = dto.FormTitle,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedAt = DateTime.UtcNow
            };

            return await _formRepo.AddFormAsync(form);
        }

        public async Task<object?> GetFormResultsAsync(int formId)
        {
            var form = await _formRepo.GetFormWithVotesAsync(formId);
            if (form == null)
                return null;

            int totalVotes = form.Votes.Count;
            int agreeVotes = form.Votes.Count(v => v.Decision);
            int disagreeVotes = totalVotes - agreeVotes;

            double agreeRate = totalVotes > 0 ? Math.Round((double)agreeVotes / totalVotes * 100, 2) : 0;

            return new
            {
                FormId = form.FormId,
                FormTitle = form.FormTitle,
                GroupId = form.GroupId,
                TotalVotes = totalVotes,
                AgreeVotes = agreeVotes,
                DisagreeVotes = disagreeVotes,
                AgreeRate = agreeRate,
                StartDate = form.StartDate,
                EndDate = form.EndDate
            };
        }

        public async Task<Form?> GetFormByIdAsync(int id)
        {
            return await _formRepo.GetByIdAsync(id);
        }
    }
}
