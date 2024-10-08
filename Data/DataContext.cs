﻿using EncryptorService.Services;
using Futbol_Insight_Jobs.Models;
using Microsoft.EntityFrameworkCore;

namespace Futbol_Insight_Jobs.Data
{
    public class DataContext : DbContext
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration, IEncryptionService encryptionService)
        {
            _configuration = configuration;
            _encryptionService = encryptionService;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("AppDatabaseConnection");
            optionsBuilder.UseMySql(_encryptionService.DecryptString(connectionString, "nil6BZYakrYZrtMw"),
                                    new MySqlServerVersion(new Version(8, 0, 21)));
        }

        public DbSet<CountryModel> countries { get; set; }
    }
}
