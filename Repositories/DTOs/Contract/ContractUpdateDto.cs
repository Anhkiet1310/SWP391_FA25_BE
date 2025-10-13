using System.ComponentModel.DataAnnotations;

namespace Repositories.DTOs.Contract
{
    public class ContractUpdateDto
    {
        public string? ContractTitle { get; set; }
        public string? ContractTerms { get; set; }
        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
    }
}