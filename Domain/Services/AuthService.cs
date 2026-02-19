using AssetLocater.Domain.Models;
using AssetLocater.Domain.Persistence;
using AssetLocater.Domain.Security;
using Microsoft.EntityFrameworkCore;

namespace AssetLocater.Domain.Services
{
    public class AuthService(FileDbContext db)
    {
        private readonly FileDbContext _db = db;

        public async Task<AppUser?> AuthenticateAsync(string username, string password)
        {
            var user = await _db.Users
               .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return null;

            return PasswordHasher.Verify(password, user.PasswordHash)
                ? user
                : null;
        }
    }
}
