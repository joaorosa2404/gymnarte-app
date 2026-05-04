using Microsoft.EntityFrameworkCore;

namespace GymnArteApp.Server.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

        public DbSet<Models.User>                 Users                 { get; set; }
        public DbSet<Models.UserScope>            UserScopes            { get; set; }
        public DbSet<Models.BiometricData>        BiometricData         { get; set; }
        public DbSet<Models.TrainingPlan>         TrainingPlans         { get; set; }
        public DbSet<Models.Exercise>             Exercises             { get; set; }
        public DbSet<Models.ExerciseTrainingPlan> ExerciseTrainingPlans { get; set; }
        public DbSet<Models.ExerciseType>         ExerciseTypes         { get; set; }
        public DbSet<Models.Notification>         Notifications         { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── User ─────────────────────────────────────────────────────────
            modelBuilder.Entity<Models.User>(e =>
            {
                e.HasKey(u => u.UserId);
                e.HasIndex(u => u.Email).IsUnique();
                e.HasIndex(u => u.UserName).IsUnique();
                e.HasIndex(u => u.PartnerNumber).IsUnique();

                e.Property(u => u.Email).HasMaxLength(256).IsRequired();
                e.Property(u => u.UserName).HasMaxLength(100).IsRequired();
                e.Property(u => u.Name).HasMaxLength(100).IsRequired();
                e.Property(u => u.Surname).HasMaxLength(100).IsRequired();
                e.Property(u => u.Phone).HasMaxLength(20);
                e.Property(u => u.Password).IsRequired();
                e.Property(u => u.Gender).HasConversion<string>();

                // Auto-referência de auditoria
                e.HasOne(u => u.UpdatedUser)
                 .WithMany()
                 .HasForeignKey(u => u.UpdatedUserId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── UserScope (1:1 com User, FK neste lado) ───────────────────────
            modelBuilder.Entity<Models.UserScope>(e =>
            {
                e.HasKey(us => us.UserScopeId);
                e.HasIndex(us => us.UserId).IsUnique(); // garante 1:1

                e.Property(us => us.Role).HasConversion<string>();

                // Principal: User — Dependent: UserScope
                e.HasOne(us => us.User)
                 .WithOne(u => u.UserScope)
                 .HasForeignKey<Models.UserScope>(us => us.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                // Auditoria
                e.HasOne(us => us.UpdatedUser)
                 .WithMany()
                 .HasForeignKey(us => us.UpdatedUserId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── BiometricData ─────────────────────────────────────────────────
            modelBuilder.Entity<Models.BiometricData>(e =>
            {
                e.HasKey(b => b.BiometricDataId);

                e.Property(b => b.Weight).HasPrecision(6, 2);
                e.Property(b => b.Height).HasPrecision(5, 2);
                e.Property(b => b.FatPercent).HasPrecision(5, 2);
                e.Property(b => b.LeanMassPercent).HasPrecision(5, 2);
                e.Property(b => b.BodyWaterPercent).HasPrecision(5, 2);
                e.Property(b => b.VisceralFat).HasPrecision(5, 2);

                e.HasOne(b => b.User)
                 .WithMany(u => u.BiometricData)
                 .HasForeignKey(b => b.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Notification ──────────────────────────────────────────────────
            modelBuilder.Entity<Models.Notification>(e =>
            {
                e.HasKey(n => n.NotificationId);
                e.Property(n => n.Title).HasMaxLength(200).IsRequired();
                e.Property(n => n.Description).IsRequired();

                e.HasOne(n => n.User)
                 .WithMany(u => u.Notifications)
                 .HasForeignKey(n => n.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── ExerciseType ──────────────────────────────────────────────────
            modelBuilder.Entity<Models.ExerciseType>(e =>
            {
                e.HasKey(et => et.ExerciseTypeId);
                e.Property(et => et.Name).HasMaxLength(100).IsRequired();
                e.Property(et => et.Description).HasMaxLength(500);

                e.HasOne(et => et.UpdatedUser)
                 .WithMany()
                 .HasForeignKey(et => et.UpdatedUserId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── Exercise ──────────────────────────────────────────────────────
            modelBuilder.Entity<Models.Exercise>(e =>
            {
                e.HasKey(ex => ex.ExerciseId);
                e.Property(ex => ex.Name).HasMaxLength(150).IsRequired();
                e.Property(ex => ex.Notes).HasMaxLength(1000);

                e.HasOne(ex => ex.ExerciseType)
                 .WithMany(et => et.Exercises)
                 .HasForeignKey(ex => ex.ExerciseTypeId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ex => ex.UpdatedUser)
                 .WithMany()
                 .HasForeignKey(ex => ex.UpdatedUserId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── TrainingPlan ──────────────────────────────────────────────────
            modelBuilder.Entity<Models.TrainingPlan>(e =>
            {
                e.HasKey(tp => tp.TrainingPlanId);
                e.Property(tp => tp.TrainingPlanName).HasMaxLength(200).IsRequired();
                e.Property(tp => tp.Notes).HasMaxLength(2000);

                e.HasOne(tp => tp.User)
                 .WithMany(u => u.TrainingPlans)
                 .HasForeignKey(tp => tp.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(tp => tp.ExerciseType)
                 .WithMany(et => et.TrainingPlans)
                 .HasForeignKey(tp => tp.ExerciseTypeId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(tp => tp.UpdatedUser)
                 .WithMany()
                 .HasForeignKey(tp => tp.UpdatedUserId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── ExerciseTrainingPlan (junção many-to-many) ────────────────────
            modelBuilder.Entity<Models.ExerciseTrainingPlan>(e =>
            {
                e.HasKey(etp => etp.ExerciseTrainingPlanId);
                e.Property(etp => etp.Notes).HasMaxLength(1000);

                e.HasOne(etp => etp.Exercise)
                 .WithMany(ex => ex.ExerciseTrainingPlans)
                 .HasForeignKey(etp => etp.ExerciseId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(etp => etp.TrainingPlan)
                 .WithMany(tp => tp.ExerciseTrainingPlans)
                 .HasForeignKey(etp => etp.TrainingPlanId)
                 .OnDelete(DeleteBehavior.Cascade);

                // Evita duplicados no mesmo plano para o mesmo exercício
                e.HasIndex(etp => new { etp.ExerciseId, etp.TrainingPlanId }).IsUnique();
            });
        }
    }
}
