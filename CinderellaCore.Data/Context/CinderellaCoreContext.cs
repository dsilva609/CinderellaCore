﻿using System.Linq;
using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.Discogs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;

namespace CinderellaCore.Data.Context
{
    public class CinderellaCoreContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Tracklist> Tracks { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<FunkoPop> Pops { get; set; }
        public DbSet<Wish> Wishes { get; set; }

        public CinderellaCoreContext(DbContextOptions<CinderellaCoreContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            
            var sqlServerOptionsExtension =
                (SqlServerOptionsExtension) builder.Options.Extensions.FirstOrDefault(x => x.GetType() == typeof(SqlServerOptionsExtension));
            if (sqlServerOptionsExtension != null)
            {
                string connectionString = sqlServerOptionsExtension.ConnectionString;
                builder.UseSqlServer(connectionString);
            }

            base.OnConfiguring(builder);
        }
    }

    //public class CinderellaCoreContextDbFactory : IDesignTimeDbContextFactory<CinderellaCoreContext>
    //{
    //    public CinderellaCoreContext CreateDbContext(string[] args)
    //    {
    //        var settings = new GlobalSettings
    //        {
    //            SQLConnectionString = "Server=.\\SQLEXPRESS;Database=CinderellaCore;Trusted_Connection=True;MultipleActiveResultSets=true"
    //        };
    //        var optionsBuilder = new DbContextOptionsBuilder<CinderellaCoreContext>();
    //        optionsBuilder.UseSqlServer<CinderellaCoreContext>(settings.SQLConnectionString);

    //        return new CinderellaCoreContext(optionsBuilder.Options);
    //    }
    //}
}