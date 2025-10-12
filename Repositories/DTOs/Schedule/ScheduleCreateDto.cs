using System.ComponentModel.DataAnnotations;

namespace Repositories.DTOs.Schedule
{
    public class ScheduleCreateDto
    {
        [Required(ErrorMessage = "CarUserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CarUserId must be greater than 0.")]
        public int CarUserId { get; set; }
        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public int Status { get; set; }
    }
}