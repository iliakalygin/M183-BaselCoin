using Microsoft.AspNetCore.Mvc;
using WebApiBaselCoin.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiBaselCoin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Context _context;

        public UserController(IConfiguration configuration, Context context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            // Log action
            var log = new AppLog
            {
                EventDate = DateTime.Now,
                EventType = "RetrieveAllUsers",
                UserId = 0, // 0 or a specific admin ID if needed
                Action = "GetAllUsers"
            };
            _context.AppLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Log action
            var log = new AppLog
            {
                EventDate = DateTime.Now,
                EventType = "RetrieveUser",
                UserId = id,
                Action = $"GetUserById: {id}"
            };
            _context.AppLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Log action
            var log = new AppLog
            {
                EventDate = DateTime.Now,
                EventType = "CreateUser",
                UserId = newUser.Id,
                Action = "UserCreated"
            };
            _context.AppLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var userToUpdate = await _context.Users.FindAsync(id);
            if (userToUpdate == null) return NotFound();

            userToUpdate.Username = user.Username;
            userToUpdate.Password_Hash = user.Password_Hash;
            userToUpdate.Role = user.Role;
            userToUpdate.Balance = user.Balance;
            await _context.SaveChangesAsync();

            // Log action
            var log = new AppLog
            {
                EventDate = DateTime.Now,
                EventType = "UpdateUser",
                UserId = id,
                Action = $"UserUpdated: {id}"
            };
            _context.AppLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // Log action
            var log = new AppLog
            {
                EventDate = DateTime.Now,
                EventType = "DeleteUser",
                UserId = id,
                Action = $"UserDeleted: {id}"
            };
            _context.AppLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
