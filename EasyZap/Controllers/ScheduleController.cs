using EasyZap.Data;
using EasyZap.Models;
using EasyZap.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyZap.Controllers
{
    public class ScheduleController : BaseController
    {
        private readonly EasyZapContext _context;
        public ScheduleController(UserService userService) : base(userService)
        {
            _context = userService.GetContext();
        }

        [HttpPost]
        public async Task<IActionResult> SaveDay([FromBody] WorkDay day)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            if (userId == null)
                return Unauthorized();

            var existing = await _context.WorkDays
                .FirstOrDefaultAsync(d => d.MasterId == userId && d.Date.Date == day.Date.Date);

            if (existing != null)
            {
                existing.StartTime = day.StartTime;
                existing.EndTime = day.EndTime;
                existing.Notes = day.Notes;
            }
            else
            {
                var newDay = new WorkDay
                {
                    MasterId = userId.Value,
                    Date = day.Date,
                    StartTime = day.StartTime,
                    EndTime = day.EndTime,
                    Notes = day.Notes
                };

                _context.WorkDays.Add(newDay);
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetDays(int year, int month)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            if (userId == null)
                return Unauthorized();

            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddDays(-1);

            var days = await _context.WorkDays
                .Where(d => d.MasterId == userId && d.Date >= start && d.Date <= end)
                .Select(d => new
                {
                    d.Date,
                    d.StartTime,
                    d.EndTime,
                    d.Notes
                })
                .ToListAsync();

            return Ok(days);
        }
    }
}
