using System.Security.Claims;
using DataAccess;
using DataAccess.UserModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyZap.Service
{
    public class UserService
    {
        private readonly EasyZapContext _context;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher = new();

        public UserService(EasyZapContext context)
        {
            _context = context;
        }

        public EasyZapContext GetContext() => _context;

        public async Task<ApplicationUser?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<ApplicationUser?> GetByIdAsync(int id) =>
            await _context.Users.FindAsync(id);

        public string HashPassword(ApplicationUser user, string password) =>
            _passwordHasher.HashPassword(user, password);

        public PasswordVerificationResult VerifyPassword(ApplicationUser user, string password) =>
            _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        public async Task SaveUserAsync(ApplicationUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task SignInAsync(ApplicationUser user, HttpContext httpContext)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public Task<bool> IsEmailTakenAsync(string email) => _context.Users.AnyAsync(u => u.Email == email);

        public int? GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out int id))
                return id;

            return null;
        }
    }
}
