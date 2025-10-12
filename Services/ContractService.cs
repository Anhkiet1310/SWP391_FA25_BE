using AutoMapper;
using Repositories;
using Repositories.DTOs.Contract;

namespace Services
{
    public class ContractService
    {
        private readonly ContractRepository _contractRepository;
        private readonly IMapper _mapper;

        public ContractService(ContractRepository contractRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        //TODO: Add business logic
        public async Task<List<ContractDto>> GetAllContracts()
        {
            var contracts = await _contractRepository.GetAllContracts();
            return _mapper.Map<List<ContractDto>>(contracts);
        }
        public async Task<ContractDto> GetContractById(int contractId)
        {
            var contract = await _contractRepository.GetContractById(contractId);
            return _mapper.Map<ContractDto>(contract);
        }
        public async Task<List<ContractDto>> GetContractsByUserId(int userId)
        {
            var contracts = await _contractRepository.GetContractsByUserId(userId);
            return _mapper.Map<List<ContractDto>>(contracts);
        }
        public async Task<ContractDto> CreateContract(ContractCreateDto contractDto)
        {
            var contract = _mapper.Map<Repositories.Entities.Contract>(contractDto);
            var createdContract = await _contractRepository.CreateContract(contract);
            return _mapper.Map<ContractDto>(createdContract);
        }
        public async Task<ContractDto?> UpdateContract(int contractId, ContractUpdateDto contractDto)
        {
            var existingContract = await _contractRepository.GetContractById(contractId);
            if (existingContract == null)
                return null;

            _mapper.Map(contractDto, existingContract);

            await _contractRepository.UpdateContract(existingContract);
            return _mapper.Map<ContractDto>(existingContract);
        }
        public async Task<bool> DeleteContract(int contractId)
        {
            var contract = await _contractRepository.GetContractById(contractId);
            if (contract == null)
                return false;
            contract.DeleteAt = DateTime.UtcNow;
            await _contractRepository.UpdateContract(contract);
            return true;
        }
    }
}