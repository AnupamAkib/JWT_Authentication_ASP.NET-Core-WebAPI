using JWT_Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_Authentication
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
