using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.Contract;
using Services;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly ContractService _contractService;

        public ContractController(ContractService contractService)
        {
            _contractService = contractService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllContracts()
        {
            try
            {
                var contracts = await _contractService.GetAllContracts();
                return Ok(contracts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContractById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid contract ID");
            }
            try
            {
                var contract = await _contractService.GetContractById(id);
                if (contract == null)
                    return NotFound(new { message = "Not found!" });

                return Ok(contract);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetContractsByUserId(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID");
            }
            try
            {
                var contracts = await _contractService.GetContractsByUserId(userId);
                return Ok(contracts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateContract(ContractCreateDto contractCreateDto)
        {
            if (contractCreateDto == null)
            {
                return BadRequest("Contract data is required");
            }
            try
            {
                var createdContract = await _contractService.CreateContract(contractCreateDto);
                return Ok(createdContract);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContract(int id, ContractUpdateDto contractUpdateDto)
        {
            if (id <= 0 || contractUpdateDto == null)
            {
                return BadRequest("Invalid input data");
            }
            try
            {
                var updatedContract = await _contractService.UpdateContract(id, contractUpdateDto);
                if (updatedContract == null)
                {
                    return NotFound(new { message = "Not found!" });
                }
                return Ok(updatedContract);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid contract ID");
            }
            try
            {
                var deleted = await _contractService.DeleteContract(id);
                if (!deleted)
                {
                    return NotFound(new { message = "Not found!" });
                }
                return Ok(new { message = "Contract deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}