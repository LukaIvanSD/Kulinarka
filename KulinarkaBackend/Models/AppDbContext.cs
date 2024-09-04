using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kulinarka.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var genderConverter = new EnumToStringConverter<Gender>();
            modelBuilder.Entity<User>()
                .Property(u => u.Gender)
                .HasConversion(genderConverter);
            base.OnModelCreating(modelBuilder);

        }
    }

}
