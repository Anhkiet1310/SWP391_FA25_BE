using System.ComponentModel.DataAnnotations;

namespace Repositories.DTOs.Schedule
{
    public class ScheduleUpdateDto
    {
        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public int Status { get; set; }
    }
}