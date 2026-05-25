using Data.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;

        public LoginRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Profile?> GetProfileByCredentialsAsync(string email, string password)
        {
            return await _context.Profiles.AsNoTracking().FirstOrDefaultAsync(profile => profile.EmailAddress == email && profile.Password == password);
        }
    }
}
