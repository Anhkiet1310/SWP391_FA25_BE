using System.ComponentModel.DataAnnotations;

namespace Repositories.DTOs.Contract
{
    public class ContractCreateDto
    {
        [Required(ErrorMessage = "CarUserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CarUserId must be greater than 0.")]
        public int CarUserId { get; set; }
        public string? ContractTitle { get; set; }
        public string? ContractTerms { get; set; }
        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
    }
}