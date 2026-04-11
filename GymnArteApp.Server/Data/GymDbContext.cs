using Microsoft.EntityFrameworkCore;

namespace GymnArteApp.Server.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.UserScope> UserScopes { get; set; }
        public DbSet<Models.BiometricData> BiometricData { get; set; }
        public DbSet<Models.TrainingPlan> TrainingPlans { get; set; }
        public DbSet<Models.Exercise> Exercises { get; set; }
        public DbSet<Models.ExerciseTrainingPlan> ExerciseTrainingPlans { get; set; }
        public DbSet<Models.ExerciseType> ExerciseTypes { get; set; }
        public DbSet<Models.Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the User entity
            //modelBuilder.Entity<Models.User>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            //    entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            //    entity.Property(e => e.PasswordHash).IsRequired();
            //});
        }
    }
}
