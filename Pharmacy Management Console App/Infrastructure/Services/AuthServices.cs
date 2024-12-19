using System.Linq;
using PharmacyManagement.Core.Interfaces;
using PharmacyManagement.Core.Models;
using PharmacyManagement.Infrastructure.Data;

namespace PharmacyManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            return _context.Users.FirstOrDefault(u =>
                u.Username == username && u.Password == password);
        }

        public void Logout()
        {
            // Có thể thêm logic logout nếu cần
        }
    }

}
