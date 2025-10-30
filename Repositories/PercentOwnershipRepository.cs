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
    public class PercentOwnershipRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public PercentOwnershipRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả PercentOwnerships
        public async Task<IEnumerable<PercentOwnership>> GetAllAsync()
        {
            return await _context.PercentOwnership
                .Where(po => po.DeleteAt == null)
                .ToListAsync();
        }

        // Lấy PercentOwnership theo ID
        public async Task<PercentOwnership?> GetByIdAsync(int id)
        {
            return await _context.PercentOwnership
                .Include(po => po.CarUser)  // Bao gồm thông tin người dùng và xe
                .FirstOrDefaultAsync(po => po.PercentOwnershipId == id && po.DeleteAt == null);
        }

        // Thêm PercentOwnership mới
        public async Task AddAsync(PercentOwnership percentOwnership)
        {
            await _context.PercentOwnership.AddAsync(percentOwnership);
        }

        // Cập nhật PercentOwnership
        public async Task UpdateAsync(PercentOwnership percentOwnership)
        {
            _context.PercentOwnership.Update(percentOwnership);
        }

        // Soft delete PercentOwnership
        public async Task DeleteAsync(PercentOwnership percentOwnership)
        {
            percentOwnership.DeleteAt = DateTime.UtcNow;  // Soft delete
            _context.PercentOwnership.Update(percentOwnership);
        }

        // Lưu thay đổi
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Kiểm tra nếu PercentOwnership đã tồn tại theo CarUserId
        public async Task<bool> PercentOwnershipExists(int carUserId)
        {
            return await _context.PercentOwnership
                .AnyAsync(po => po.CarUserId == carUserId && po.DeleteAt == null);
        }
    }
}

