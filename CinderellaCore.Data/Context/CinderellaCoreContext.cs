using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.Discogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace CinderellaCore.Data.Context
{
	public class CinderellaCoreContext : DbContext
	{
		public DbSet<Album> Albums { get; set; }
		public DbSet<Tracklist> Tracks { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<Game> Games { get; set; }
		public DbSet<FunkoPop> Pops { get; set; }
		public DbSet<Wish> Wishes { get; set; }

		private readonly GlobalSettings _settings;

		public CinderellaCoreContext(DbContextOptions<CinderellaCoreContext> options, GlobalSettings settings) : base(options)
		{
			_settings = settings;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			var sqlServerOptionsExtension = builder.Options.FindExtension<SqlServerOptionsExtension>();
			if (sqlServerOptionsExtension != null)
			{
				string connectionString = sqlServerOptionsExtension.ConnectionString;
				builder.UseSqlServer(connectionString);
			}

			base.OnConfiguring(builder);
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