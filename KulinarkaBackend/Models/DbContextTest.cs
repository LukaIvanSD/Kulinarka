using Microsoft.EntityFrameworkCore;

namespace Kulinarka.Models
{
    public class DbContextTest:DbContext
    {
        public DbContextTest(DbContextOptions<DbContextTest> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
