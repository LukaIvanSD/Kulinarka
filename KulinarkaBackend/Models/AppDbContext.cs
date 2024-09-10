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
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<UserAchievement> UserAchievement { get; set; }
        public DbSet<UserTitle> UserTitle { get; set; }
        public DbSet<UserStatistic> UserStatistics { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var genderConverter = new EnumToStringConverter<Gender>();
            modelBuilder.Entity<User>()
                .Property(u => u.Gender)
                .HasConversion(genderConverter);
            var difficultyConverter = new EnumToStringConverter<Difficulty>();
            modelBuilder.Entity<Recipe>().Property(recipe=>recipe.Difficulty).HasConversion(difficultyConverter);
            modelBuilder.Entity<Recipe>()
       .Property(r => r.Duration) 
       .HasColumnType("time(7)");
            modelBuilder.Entity<UserAchievement>()
            .HasKey(ua => new { ua.UserId, ua.AchievementId });
            var requirementTypeConverter = new EnumToStringConverter<RequirementType>();
            modelBuilder.Entity<Achievement>()
                .Property(a => a.RequirementType)
                .HasConversion(requirementTypeConverter);

            modelBuilder.Entity<UserAchievement>()
         .HasOne(ua => ua.User)
         .WithMany(u => u.UserAchievements)
         .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.Achievement)
                .WithMany(a => a.UserAchievements)
                .HasForeignKey(ua => ua.AchievementId);
            var titleTypeConverter = new EnumToStringConverter<TitleType>();

            modelBuilder.Entity<Title>()
                .Property(t => t.TitleType)
                .HasConversion(titleTypeConverter);
            modelBuilder.Entity<UserTitle>()
                .HasOne(ut => ut.CurrentTitle)
                .WithMany(t=>t.UserTitles)
                .HasForeignKey(ut => ut.TitleId);
            modelBuilder.Entity<User>()
              .HasOne(u => u.UserTitle)
              .WithOne(ut => ut.User)
              .HasForeignKey<UserTitle>(ut => ut.UserId);
            modelBuilder.Entity<UserStatistic>()
                .HasOne(us => us.User)
                .WithOne(u => u.UserStatistic)
                .HasForeignKey<UserStatistic>(us => us.UserId);
            base.OnModelCreating(modelBuilder);

        }
    }

}
