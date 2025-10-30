using Repositories;
using Repositories.DTOs.PercentOwnership;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PercentOwnershipService
    {
        private readonly PercentOwnershipRepository _percentOwnershipRepository;

        public PercentOwnershipService(PercentOwnershipRepository percentOwnershipRepository)
        {
            _percentOwnershipRepository = percentOwnershipRepository;
        }

        // Lấy tất cả PercentOwnerships
        public async Task<IEnumerable<PercentOwnership>> GetAllPercentOwnershipsAsync()
        {
            return await _percentOwnershipRepository.GetAllAsync();
        }

        // Lấy PercentOwnership theo ID
        public async Task<PercentOwnership?> GetPercentOwnershipByIdAsync(int id)
        {
            return await _percentOwnershipRepository.GetByIdAsync(id);
        }

        // Tạo PercentOwnership mới
        public async Task<PercentOwnership> CreatePercentOwnershipAsync(PercentOwnershipCreateDto dto)
        {
            var percentOwnership = new PercentOwnership
            {
                CarUserId = dto.CarUserId,
                Percentage = dto.Percentage,
                UsageLimit = dto.UsageLimit,
                CreatedAt = DateTime.UtcNow
            };

            await _percentOwnershipRepository.AddAsync(percentOwnership);
            await _percentOwnershipRepository.SaveChangesAsync();

            return percentOwnership;
        }

        // Cập nhật PercentOwnership
        public async Task<PercentOwnership> UpdatePercentOwnershipAsync(int id, PercentOwnershipUpdateDto dto)
        {
            var percentOwnership = await _percentOwnershipRepository.GetByIdAsync(id);
            if (percentOwnership == null)
                throw new Exception($"PercentOwnership with id {id} not found.");

            percentOwnership.Percentage = dto.Percentage ?? percentOwnership.Percentage;
            percentOwnership.UsageLimit = dto.UsageLimit ?? percentOwnership.UsageLimit;
            percentOwnership.UpdatedAt = DateTime.UtcNow;

            await _percentOwnershipRepository.UpdateAsync(percentOwnership);
            await _percentOwnershipRepository.SaveChangesAsync();

            return percentOwnership;
        }

        // Xóa PercentOwnership
        public async Task<bool> DeletePercentOwnershipAsync(int id)
        {
            var percentOwnership = await _percentOwnershipRepository.GetByIdAsync(id);
            if (percentOwnership == null)
                return false;

            await _percentOwnershipRepository.DeleteAsync(percentOwnership);
            await _percentOwnershipRepository.SaveChangesAsync();

            return true;
        }
    }
}

