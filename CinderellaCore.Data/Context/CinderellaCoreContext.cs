using CinderellaCore.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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

    public class CinderellaCoreContextDbFactory : IDesignTimeDbContextFactory<CinderellaCoreContext>
    {
        public CinderellaCoreContext CreateDbContext(string[] args)
        {
            var settings = new GlobalSettings
            {
                SQLConnectionString = "Server=.\\SQLEXPRESS;Database=CinderellaCore;Trusted_Connection=True;MultipleActiveResultSets=true"
            };
            var optionsBuilder = new DbContextOptionsBuilder<CinderellaCoreContext>();
            optionsBuilder.UseSqlServer<CinderellaCoreContext>(settings.SQLConnectionString);

            return new CinderellaCoreContext(optionsBuilder.Options, settings);
        }
    }
}