using Microsoft.EntityFrameworkCore;
using Timelogger.Entities;

namespace Timelogger
{
	public class ApiContext : DbContext
	{
		public ApiContext(DbContextOptions<ApiContext> options)
			: base(options)
		{
		}

		public DbSet<Project> Projects { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Activity> Activity { get; set; }
		public DbSet<FeatureSettings> FeatureSettings { get; set; }
		public DbSet<ActivityRegister> ActivityRegister { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Create a unique index on the Identifier field 
			modelBuilder.Entity<FeatureSettings>()
				.HasIndex(f => f.Identifier)
				.IsUnique(); 
		}
	}
}
