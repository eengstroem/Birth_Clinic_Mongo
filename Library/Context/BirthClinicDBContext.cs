using Library.Models.Births;
using Library.Models.Clinicians;
using Library.Models.FamilyMembers;
using Library.Models.Reservations;
using Library.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Library.Context
{
    public class BirthClinicDbContext : DbContext
    {

        public BirthClinicDbContext(DbContextOptions<BirthClinicDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Birth> Births { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }

        public DbSet<Room> Rooms { get; set; }
      
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Clinician> Clinicians { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // -- FamilyMember Hierarchy table --

            modelBuilder.Entity<FamilyMember>()
                .HasDiscriminator<string>("blog_type")
                .HasValue<Father>("father")
                .HasValue<Child>("child")
                .HasValue<Relative>("relative")
                .HasValue<Mother>("mother");

            // -- Birth Relationships --


            modelBuilder.Entity<Birth>()
                .HasMany<Clinician>(b => b.AssociatedClinicians)
                .WithMany(c => c.AssignedBirths)
                .UsingEntity(j => j.ToTable("ClinicianBirthJoins"));

            modelBuilder.Entity<Birth>()
                .HasMany<Child>(b => b.ChildrenToBeBorn)
                .WithOne(c => c.AssociatedBirth)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Birth>()
                .HasOne<Mother>(b => b.Mother)
                .WithOne(m => m.AssociatedBirth)
                .HasForeignKey<Birth>(b => b.MotherForeignKey)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Birth>()
                .HasOne<Father>(b => b.Father)
                .WithOne(f => f.AssociatedBirth)
                .HasForeignKey<Birth>(b => b.FatherForeignKey)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Birth>()
                .HasMany<Relative>(c => c.Relatives)
                .WithOne(f => f.AssociatedBirth)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            // -- Reservation Relationships --

            modelBuilder.Entity<Reservation>()
                .HasOne(c => c.AssociatedBirth)
                .WithMany()
                .HasForeignKey(c => c.BirthId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(c => c.ReservedRoom)
                .WithMany(c => c.CurrentReservations)
                .OnDelete(DeleteBehavior.NoAction);
            



        }
    }
}
