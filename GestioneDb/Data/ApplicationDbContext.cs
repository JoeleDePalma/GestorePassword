using Microsoft.EntityFrameworkCore;
using GestioneDb.Models;

namespace GestioneDb.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<Password>().HasKey(p => p.CredentialID);
            modelbuilder.Entity<Password>().Property(p => p.CredentialID).ValueGeneratedOnAdd();

            modelbuilder.Entity<User>().HasKey(u => u.UserID);
            modelbuilder.Entity<User>().Property(u => u.UserID).ValueGeneratedOnAdd();

        }

        public DbSet<Password> Passwords { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
