using CinderellaCore.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace CinderellaCore.Web.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
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

		public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
		{
			public ApplicationDbContext CreateDbContext(string[] args)
			{
				var settings = new GlobalSettings
				{
					//prod string
					SQLConnectionString = "Server=.\\SQLEXPRESS;Database=CinderellaCore;Trusted_Connection=True;MultipleActiveResultSets=true"
				};
				var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
				optionsBuilder.UseSqlServer<ApplicationDbContext>(settings.SQLConnectionString);

				return new ApplicationDbContext(optionsBuilder.Options);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);
		}
	}
}