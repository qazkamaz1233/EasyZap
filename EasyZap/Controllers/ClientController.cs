using System.Security.Claims;
using EasyZap.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyZap.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly EasyZapContext _context;

        public ClientController(EasyZapContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out var useId))
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == useId);
                return View(user);
            }

            return Unauthorized();
        }
    }
}
