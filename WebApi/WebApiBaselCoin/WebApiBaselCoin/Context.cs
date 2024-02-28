using Microsoft.EntityFrameworkCore;
using WebApiBaselCoin.Models;

namespace WebApiBaselCoin
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
      : base(options)
        {
        }
        // Database
        public DbSet<User> Users { get; set; }
    }
}
