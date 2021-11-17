using Microsoft.EntityFrameworkCore;
using TripAgency.DataAccessLayer.Models;

namespace TripAgency.DataAccessLayer.Repository.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<Client> Client { get; set; }

        public virtual DbSet<Sale> Sale { get; set; }

        public virtual DbSet<Trip> Trip { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-ALEKSEY\\SQLEXPRESS;Initial Catalog=TripAgency;Integrated Security=True");
            }
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DatabaseContext()
        {

        }
    }
}
