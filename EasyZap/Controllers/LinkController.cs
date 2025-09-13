using EasyZap.Service;
using Microsoft.AspNetCore.Mvc;

namespace EasyZap.Controllers
{
    [Route("Link")]
    public class LinkController : BaseController
    {
        private readonly LinkService _linkService;
        private IConfiguration _config;
        public LinkController(LinkService linkService, UserService userService, IConfiguration config) : base(userService) 
        {
            _linkService = linkService;
            _config = config;
        }

        [HttpPost("Generate")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate()
        {
            var userId = GetCurrentUserId();
            if(userId == null) return Unauthorized();

            var inv = await _linkService.CreateMasterLinkAsync(userId.Value);

            var botName = _config["Telegram:BotUsername"] ?? "Bot";
            var link = $"https://t.me/{botName}?start={inv.Token}";

            return Ok(new { link, token = inv.Token, createdAt = inv.CreatedAt });
        }

        [HttpPost("Revoke")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke([FromForm] string token)
        {
            var userId = GetCurrentUserId();
            if(userId == null) return Unauthorized();

            var existing = await _linkService.GetByTokenAsync(token);
            if(existing == null || existing.MasterId != userId.Value) return Forbid();

            await _linkService.RevokeTokenAsync(token);
            return Ok(new { success = true});
        }
    }
}
