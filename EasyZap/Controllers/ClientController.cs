using System.Security.Claims;
using EasyZap.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyZap.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : BaseController
    {
        public ClientController(UserService userService) : base(userService) { }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out var useId))
            {
                var user = await _userService.GetByIdAsync(useId);
                return View(user);
            }

            return Unauthorized();
        }
    }
}
