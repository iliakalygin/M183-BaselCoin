using Microsoft.AspNetCore.Mvc;
using WebApiBaselCoin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebApiBaselCoin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Context _context;

        public UserController(Context context)
        {
            _context = context;
        }

        // Verwendung von EF Core's ToListAsync, um Daten sicher abzufragen und SQL-Injection zu vermeiden
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // Verwendung von EF Core's FindAsync für parameterisierte Abfragen, die vor SQL-Injection schützen
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // Verwendung von EF Core zum Hinzufügen von Daten, bietet Schutz vor SQL-Injection
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // Verwendung von EF Core zum Aktualisieren von Daten, schützt vor SQL-Injection
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Verwendung von EF Core zum Löschen von Daten, bietet ebenfalls Schutz vor SQL-Injection
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
