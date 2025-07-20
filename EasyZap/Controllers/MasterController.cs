using Microsoft.AspNetCore.Mvc;
using EasyZap.Data;
using EasyZap.Models;

namespace EasyZap.Controllers
{
    public class MasterController : Controller
    {
        private readonly EasyZapContext _context;

        public MasterController(EasyZapContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Master/Register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [Route("Master/Register")]
        public IActionResult Register(Master master)
        {
            if (ModelState.IsValid)
            {
                _context.Masters.Add(master);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(master);
        }

        [HttpGet]
        public IActionResult TestRegister()
        {
            var master = new Master
            {
                Email = "test@barber.com",
                Id = 1,
                PasswordHash = "test123",
                Name = "Анна",
                Schedule = "10:00-20:00, Вс: выходной",
                Service = "Стрижка: 1000, окрашивание: 3000"
            };

            _context.Masters.Add(master);
            _context.SaveChanges();

            return Content("Мастер добавлен!");
        }
    }
}
