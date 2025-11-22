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

            modelbuilder.Entity<Password>().HasKey(p => p.Id);
            modelbuilder.Entity<Password>().Property(p => p.Id).ValueGeneratedOnAdd();
        }

        public DbSet<Password> Passwords { get; set; }
    }
}
