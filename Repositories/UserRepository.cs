using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.DTOs.User;
using Repositories.Entities;

namespace Repositories
{
    public class UserRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public UserRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User> Register(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await GetUserByUsername(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }
    }
}
