using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.Entities;

namespace Repositories
{
    public class PaymentRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public PaymentRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> GetPaymentByOrderId(string orderId)
        {
            return await _context.Payments
                         .Include(p => p.Transactions)
                         .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
}
