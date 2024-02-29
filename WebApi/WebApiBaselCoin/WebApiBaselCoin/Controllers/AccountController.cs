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
        private readonly Context _context; // Hinzufügen des Context

        // Konstruktor mit Context und TokenService Injektion
        public AccountController(ITokenService tokenService, Context context)
        {
            _tokenService = tokenService;
            _context = context; // Initialisieren des Context
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUser model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password_Hash == model.Password);

            if (user == null)
                return Unauthorized("Invalid Credentials");

            var token = _tokenService.CreateToken(user.Username); // Erstellen des Tokens
            var userType = user.Role.Equals("admin") ? "Admin" : "User"; // Bestimmen des Benutzertyps

            return new JsonResult(new { token = token, userType = userType }); // Rückgabe des Tokens und des Benutzertyps
        }

    }
}
