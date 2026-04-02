using Microsoft.EntityFrameworkCore;
using GestioneDb.Models;

namespace GestioneDb.Data
{
    /// <summary>
    /// Represents the application's database context and manages the entity configurations
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes the database context with the specified options
        /// </summary>
        /// <param name="options">The options used to configure the context</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Password>().HasKey(p => p.CredentialID);
            modelBuilder.Entity<Password>().Property(p => p.CredentialID).ValueGeneratedOnAdd();

            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<User>().Property(u => u.UserID).ValueGeneratedOnAdd();
        }

        public DbSet<Password> Passwords { get; set; }
        public DbSet<User> Users { get; set; }
    }
}