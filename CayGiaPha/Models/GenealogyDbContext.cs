using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CayGiaPha.Models
{
    public class GenealogyDbContext : IdentityDbContext<Users, IdentityRole<int>, int>
    {
        public GenealogyDbContext(DbContextOptions<GenealogyDbContext> options)
            : base(options)
        {
        }

        // === DbSets ===
        public DbSet<Families> Families { get; set; }
        public DbSet<People> People { get; set; }
        public DbSet<ParentChild> ParentChildren { get; set; }
        public DbSet<Marriages> Marriages { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === FAMILY - PEOPLE ===
            modelBuilder.Entity<People>()
                .HasOne(p => p.Family)
                .WithMany(f => f.People)
                .HasForeignKey(p => p.FamilyID)
                .OnDelete(DeleteBehavior.Restrict);

            // === PARENT - CHILD (khắc phục lỗi thiếu khóa chính) ===
            modelBuilder.Entity<ParentChild>(entity =>
            {
                entity.HasKey(pc => new { pc.ParentID, pc.ChildID }); // ✅ Bắt buộc có khóa chính

                entity.HasOne(pc => pc.Parent)
                    .WithMany(p => p.ParentChildren)
                    .HasForeignKey(pc => pc.ParentID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pc => pc.Child)
                    .WithMany(p => p.ChildParents)
                    .HasForeignKey(pc => pc.ChildID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === MARRIAGE RELATIONSHIP ===
            modelBuilder.Entity<Marriages>(entity =>
            {
                entity.HasOne(m => m.Spouse1)
                    .WithMany(p => p.Marriages1)
                    .HasForeignKey(m => m.Spouse1ID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Spouse2)
                    .WithMany(p => p.Marriages2)
                    .HasForeignKey(m => m.Spouse2ID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === EVENTS ===
            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasOne(e => e.Organizer)
                    .WithMany(p => p.OrganizedEvents)
                    .HasForeignKey(e => e.OrganizerID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === EVENT REGISTRATION ===
            // === EVENT REGISTRATION ===
            modelBuilder.Entity<EventRegistration>(entity =>
            {
                entity.HasOne(er => er.Event)
                    .WithMany(e => e.Registrations)
                    .HasForeignKey(er => er.EventID)
                    .OnDelete(DeleteBehavior.Cascade); // khi xóa sự kiện, xóa luôn đăng ký

                entity.HasOne(er => er.Person)
                    .WithMany(p => p.EventRegistrations)
                    .HasForeignKey(er => er.PersonalID)
                    .OnDelete(DeleteBehavior.Restrict); // ✅ đổi từ Cascade → Restrict
            });

            // === USER - PERSON ONE-TO-ONE ===
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasOne(u => u.Person)
                    .WithOne(p => p.User)
                    .HasForeignKey<Users>(u => u.PersonalID)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
