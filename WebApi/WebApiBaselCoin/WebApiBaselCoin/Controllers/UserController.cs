using Microsoft.AspNetCore.Mvc;
using WebApiBaselCoin.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebApiBaselCoin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection"); // Ensure you have this in your appsettings.json
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            List<User> users = new List<User>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM users;", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = (int)reader["id"],
                        Username = reader["username"].ToString(),
                        Role = reader["role"].ToString(),
                        Balance = (decimal)reader["balance"]
                    });
                }
                connection.Close();
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUserById(int id)
        {
            User user = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM users WHERE id = @id;", connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        Id = (int)reader["id"],
                        Username = reader["username"].ToString(),
                        Role = reader["role"].ToString(),
                        Balance = (decimal)reader["balance"]
                    };
                }
                connection.Close();
            }
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand($"INSERT INTO users (username, password_hash, role, balance) VALUES (@username, @password_hash, @role, @balance);", connection);
                command.Parameters.AddWithValue("@username", newUser.Username);
                command.Parameters.AddWithValue("@password_hash", newUser.Password_Hash);
                command.Parameters.AddWithValue("@role", newUser.Role);
                command.Parameters.AddWithValue("@balance", newUser.Balance);

                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();

                if (result < 0) return StatusCode(500, "An error occurred while creating the user.");
            }
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand($"UPDATE users SET username = @username, password_hash = @password_hash, role = @role, balance = @balance WHERE id = @id;", connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@password_hash", user.Password_Hash);
                command.Parameters.AddWithValue("@role", user.Role);
                command.Parameters.AddWithValue("@balance", user.Balance);

                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();

                if (result < 0) return StatusCode(500, "An error occurred while updating the user.");
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand($"DELETE FROM users WHERE id = @id;", connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();

                if (result < 0) return StatusCode(500, "An error occurred while deleting the user.");
            }
            return Ok();
        }
    }
}
