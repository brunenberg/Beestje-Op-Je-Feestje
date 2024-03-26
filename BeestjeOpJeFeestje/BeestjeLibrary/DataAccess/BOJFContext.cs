using BeestjeLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeestjeLibrary.DataAccess {

    public class BOJFContext : IdentityDbContext<ApplicationUser, IdentityRole, string> {
        public BOJFContext() { }
        public BOJFContext(DbContextOptions<BOJFContext> options) : base(options) { }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<AnimalType> AnimalTypes { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<CustomerCard> CustomerCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Account)
                .WithOne(u => u.ApplicationUser)
                .HasForeignKey<Account>(u => u.Id);

            // Optioneel: Als je een relatie tussen Address en Account wilt definiëren, kun je dit ook toevoegen
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Address)
                .WithOne()
                .HasForeignKey<Account>(a => a.AddressId)
                .OnDelete(DeleteBehavior.Restrict); // Of een ander gewenst gedrag bij het verwijderen

            // Optioneel: Als je een relatie tussen ApplicationUser en Account wilt definiëren, kun je dit ook toevoegen
            modelBuilder.Entity<Account>()
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.Account)
                .HasForeignKey<ApplicationUser>(u => u.AccountId)
                .OnDelete(DeleteBehavior.Cascade); // Of een ander gewenst gedrag bij het verwijderen
        }


    }
}
