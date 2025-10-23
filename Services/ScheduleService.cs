using AutoMapper;
using Repositories;
using Repositories.DTOs.Schedule;

namespace Services
{
    public class ScheduleService
    {
        private readonly ScheduleRepository _scheduleRepository;
        private readonly IMapper _mapper;
        public ScheduleService(ScheduleRepository scheduleRepository, IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
        }

        public async Task<List<ScheduleDto>> GetAllSchedules()
        {
            var schedules = await _scheduleRepository.GetAll();
            return _mapper.Map<List<ScheduleDto>>(schedules);
        }
        public async Task<ScheduleDto> GetScheduleById(int scheduleId)
        {
            var schedule = await _scheduleRepository.GetById(scheduleId);
            return _mapper.Map<ScheduleDto>(schedule);
        }
        public async Task<List<ScheduleDto>> GetSchedulesByUserId(int userId)
        {
            var schedules = await _scheduleRepository.GetByUserId(userId);
            return _mapper.Map<List<ScheduleDto>>(schedules);
        }
        public async Task<ScheduleDto> CreateSchedule(ScheduleCreateDto scheduleDto)
        {
            var schedule = _mapper.Map<Repositories.Entities.Schedule>(scheduleDto);
            var createdSchedule = await _scheduleRepository.CreateSchedule(schedule);
            return _mapper.Map<ScheduleDto>(createdSchedule);
        }
        public async Task<ScheduleDto?> UpdateSchedule(int scheduleId, ScheduleUpdateDto scheduleDto)
        {
            var existingSchedule = await _scheduleRepository.GetById(scheduleId);
            if (existingSchedule == null)
                return null;

            _mapper.Map(scheduleDto, existingSchedule);

            await _scheduleRepository.UpdateSchedule(existingSchedule);
            return _mapper.Map<ScheduleDto>(existingSchedule);
        }
        public async Task<bool> DeleteSchedule(int scheduleId)
        {
            var schedule = await _scheduleRepository.GetById(scheduleId);
            if (schedule == null)
                return false;
            schedule.DeleteAt = DateTime.UtcNow;
            await _scheduleRepository.UpdateSchedule(schedule);
            return true;
        }
    }
}