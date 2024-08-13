using Futbol_Insight_Jobs.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Futbol_Insight_Jobs.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<CountryModel> countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("AppDatabaseConnection");
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
        }
    }

    public class AdmonContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AdmonContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<UserModel> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("AdmonDatabaseConnection");
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
        }
    }
}
