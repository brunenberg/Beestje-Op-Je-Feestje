using Microsoft.EntityFrameworkCore;

namespace Models {
    public class ApplicationDbContext : DbContext {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BeestjeOpJeFeestjeDb;Trusted_Connection=True;");
            }
        }
    }
}
