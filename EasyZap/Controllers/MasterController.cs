using EasyZap.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyZap.Controllers
{
    [Authorize(Roles = "Master")]
    public class MasterController : BaseController
    {
        public MasterController(UserService userService) : base(userService) { }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = _userService.GetUserIdFromClaims(User);

            if (userId == null)
                return Unauthorized();

            var user = await _userService.GetByIdAsync(userId.Value);

            if(user == null || user.Role != DataAccess.UserModel.UserRole.Master)
                return Unauthorized();

            return View(user);
        }
    }
}
