using EasyZap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EasyZap.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return Content("Регистрация прошла успешно!");
        }
    }
}
