using CinderellaCore.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace CinderellaCore.Data.Context
{
    public class CinderellaCoreContext : DbContext
    {
        private readonly GlobalSettings _settings;
        public DbSet<TestObj> Tests { get; set; }

        public CinderellaCoreContext(DbContextOptions<CinderellaCoreContext> options, GlobalSettings settings) : base(options)
        {
            _settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_settings.SQLConnectionString);
        }
    }
}