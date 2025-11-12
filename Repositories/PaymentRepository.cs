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

        public IQueryable<Payment> GetAllPaymentQuery()
        {
            return _context.Payments
                           .Include(p => p.Transactions);
            //.Include(p => p.CarUser)
            //     .ThenInclude(cu => cu.User)
            //.Include(p => p.CarUser)
            //     .ThenInclude(cu => cu.Car);
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> GetPaymentByOrderId(string orderId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
