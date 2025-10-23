using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.Entities;

namespace Repositories
{

    public class ContractRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public ContractRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contract>> GetAllContracts()
        {
            return await _context.Contracts.Where(c => c.DeleteAt == null).ToListAsync();
        }

        public async Task<Contract?> GetContractById(int contractId)
        {
            return await _context.Contracts.FirstOrDefaultAsync(c => c.ContractId == contractId && c.DeleteAt == null);
        }

        public async Task<List<Contract>> GetContractsByUserId(int userId)
        {
            var contracts = await (from c in _context.Contracts
                                   join cu in _context.CarUsers on c.CarUserId equals cu.CarUserId
                                   where cu.UserId == userId && c.DeleteAt == null
                                   select c)
                                  .ToListAsync();

            return contracts;
        }

        public async Task<Contract> CreateContract(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task<Contract> UpdateContract(Contract contract)
        {
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
            return contract;
        }
    }
}