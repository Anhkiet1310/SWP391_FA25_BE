using Repositories;
using Repositories.DBContext;
using Repositories.DTOs.Group;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GroupService
    {
        private readonly GroupRepository _groupRepository;

        public GroupService(GroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        // Tạo Group mới
        public async Task<Group> CreateGroupAsync(GroupCreateDto dto)
        {
            // Kiểm tra nếu xe đã có nhóm
            if (await _groupRepository.GroupExists(dto.CarId))
                throw new Exception($"Car with id {dto.CarId} already has a group.");

            var group = new Group
            {
                CarId = dto.CarId,
                GroupName = dto.GroupName,
                GroupImg = dto.GroupImg,
                CreatedAt = DateTime.UtcNow
            };

            await _groupRepository.AddAsync(group);
            await _groupRepository.SaveChangesAsync();

            return group;
        }

        // Cập nhật Group
        public async Task<Group> UpdateGroupAsync(int id, GroupUpdateDto dto)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null)
                throw new Exception($"Group with id {id} not found.");

            group.GroupName = dto.GroupName ?? group.GroupName;
            group.GroupImg = dto.GroupImg ?? group.GroupImg;
            group.UpdatedAt = DateTime.UtcNow;

            await _groupRepository.UpdateAsync(group);
            await _groupRepository.SaveChangesAsync();

            return group;
        }

        // Soft Delete Group
        public async Task<bool> DeleteGroupAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null)
                return false;

            await _groupRepository.DeleteAsync(group);
            await _groupRepository.SaveChangesAsync();

            return true;
        }

        // Lấy tất cả nhóm
        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _groupRepository.GetAllAsync();
        }

        // Lấy nhóm theo ID
        public async Task<Group?> GetGroupByIdAsync(int id)
        {
            return await _groupRepository.GetByIdAsync(id);
        }
    }
}


