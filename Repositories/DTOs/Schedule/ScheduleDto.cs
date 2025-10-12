using Repositories.Entities;

namespace Repositories.DTOs.Schedule
{
    public class ScheduleDto : BasicElement
    {
        public int ScheduleId { get; set; }
        public int CarUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        //public CarUser CarUser { get; set; }
    }
}