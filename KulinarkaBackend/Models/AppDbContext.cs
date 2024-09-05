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
        public DbSet<Recipe> Recipes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var genderConverter = new EnumToStringConverter<Gender>();
            modelBuilder.Entity<User>()
                .Property(u => u.Gender)
                .HasConversion(genderConverter);
            var difficultyConverter = new EnumToStringConverter<Difficulty>();
            modelBuilder.Entity<Recipe>().Property(recipe=>recipe.Difficulty).HasConversion(difficultyConverter);
            base.OnModelCreating(modelBuilder);

        }
    }

}
