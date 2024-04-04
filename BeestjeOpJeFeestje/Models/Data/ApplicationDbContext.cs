using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Models {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }
        public DbSet<CustomerCard> CustomerCards { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BeestjeOpJeFeestjeDb;Trusted_Connection=True;");
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-one relationship between ApplicationUser and Address
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Address)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.AddressId);

            // Configure the one-to-one relationship between ApplicationUser and CustomerCard
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.CustomerCard)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.CustomerCardId);
        }
    }
}
