using EasyZap.Service;
using Microsoft.AspNetCore.Mvc;

namespace EasyZap.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly UserService _userService;

        protected BaseController(UserService userService)
        {
            _userService = userService;
        }

        protected int? GetCurrentUserId()
        {
            return _userService.GetUserIdFromClaims(User);
        }
    }
}
