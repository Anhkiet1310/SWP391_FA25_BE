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
    public class FormRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public FormRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        public async Task<Form> AddFormAsync(Form form)
        {
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<Form?> GetByIdAsync(int id)
        {
            return await _context.Forms
                .Include(f => f.Group)
                .Include(f => f.Votes)
                .FirstOrDefaultAsync(f => f.FormId == id);
        }

        public async Task<Form?> GetFormWithVotesAsync(int formId)
        {
            return await _context.Forms
                .Include(f => f.Votes)
                .FirstOrDefaultAsync(f => f.FormId == formId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Forms.AnyAsync(f => f.FormId == id);
        }
    }
}
