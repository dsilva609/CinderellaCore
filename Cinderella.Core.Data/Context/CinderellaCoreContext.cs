using CinderellaCore.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinderella.Core.Data.Context
{
	public class CinderellaCoreContext : DbContext
	{
		public CinderellaCoreContext(DbContextOptions<CinderellaCoreContext> options) : base(options)
		{
		}

		public DbSet<TestObj> Tests { get; set; }
	}
}