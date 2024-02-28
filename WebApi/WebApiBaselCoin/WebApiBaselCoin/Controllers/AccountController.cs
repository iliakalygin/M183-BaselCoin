using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApiBaselCoin.Models;
using WebApiBaselCoin.Services;
using System.Threading.Tasks;

namespace WebApiBaselCoin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly Context _context;

        public AccountController(ITokenService tokenService, Context context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUser model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(e => e.Username == model.Username && e.Password_Hash == model.Password);

            if (user == null)
            {
                await LogActionAsync(0, "Login", "Invalid Credentials"); // Log unsuccessful login attempt
                return Unauthorized("Invalid Credentials");
            }

            await LogActionAsync(user.Id, "Login", "Success"); // Log successful login
            return new JsonResult(new { userName = model.Username, token = _tokenService.CreateToken(model.Username) });
        }

        private async Task LogActionAsync(int userId, string action, string result)
        {
            var log = new AppLog
            {
                EventDate = DateTime.UtcNow,
                EventType = action,
                UserId = userId,
                Action = result
            };

            _context.AppLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
