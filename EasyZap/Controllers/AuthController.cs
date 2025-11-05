using DataAccess.UserModel;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using EasyZap.Service;

namespace EasyZap.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(UserService userService) : base(userService) { }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            if (await _userService.IsEmailTakenAsync(email))
            {
                return Json($"Пользователь с таким Email уже существует");
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            if(string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "Введите имя");
                return View(model);
            }

            if(await _userService.IsEmailTakenAsync(model.Email))
            {
                ModelState.AddModelError("Email", "Пользователь с таким Email уже существует");
                return View(model);
            }

            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role
            };

            user.PasswordHash = _userService.HashPassword(user, model.Password);

            await _userService.SaveUserAsync(user);

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
            
            var user = await _userService.GetByEmailAsync(model.Email); 

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return View(model);
            }

            var result = _userService.VerifyPassword(user, model.Password);
            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Неверный пароль");
                return View(model);
            }

            await _userService.SignInAsync(user, HttpContext);

            return RedirectByRole(user.Role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectByRole(UserRole role)
        {
            return role switch
            {
                UserRole.Master => RedirectToAction("Dashboard", "Master"),
                UserRole.Client => RedirectToAction("Dashboard", "Client"),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}
