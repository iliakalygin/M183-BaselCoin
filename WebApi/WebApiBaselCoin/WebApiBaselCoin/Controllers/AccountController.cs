using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBaselCoin.Models;
using WebApiBaselCoin.Services;

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
            var employee = await _context.Users.FirstOrDefaultAsync(e => e.Username == model.Username && e.Password_Hash == model.Password);

            if (employee == null)
                return Unauthorized("Invalid Credentials");

            var token = _tokenService.CreateToken(employee.Username);
            return new JsonResult(new { userName = employee.Username, token = token });
        }
    }
}
