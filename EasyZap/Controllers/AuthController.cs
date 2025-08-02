using EasyZap.Data;
using EasyZap.Models;
using EasyZap.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EasyZap.Controllers
{
    public class AuthController : Controller
    {
        private readonly EasyZapContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public AuthController(EasyZapContext context, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return View(model);
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Неверный пароль");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); // создает информацию о пользователе, кто он и т.д.
            var principial = new ClaimsPrincipal(identity); // оборачивает identity, то что прикрепляется к пользователю

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principial); // для распознования пользователя по куке

            if (user.Role == UserRole.Master)
                return RedirectToAction("Dashboard", "Master");

            else if (user.Role == UserRole.Client)
                return RedirectToAction("Dashboard", "Client");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
