using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.DataInfrastructure
{
	public class PredictionDbContext : IdentityDbContext 
	{
		public PredictionDbContext(DbContextOptions<PredictionDbContext> options) : base(options) 
		{
		}

		// add tables of the database as the dbcontext properties here.

		//public DbSet<Threat> Threats { get; set; }

		/// <summary>
		/// Override method on creation of the tables of the database
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.HasDefaultSchema(schema: DBGlobals.SchemaName);
			//modelBuilder.Entity<Threat>().HasIndex(i => i.Referer).IsUnique();
			//modelBuilder.Entity<Threat>()
			//  .Property(p => p.Identifier).HasComputedColumnSql("CONCAT('" + DBGlobals.IdentifierFormat + "',[Id])");
			//modelBuilder.Entity<ThreatType>();
			//modelBuilder.Entity<Status>();
			base.OnModelCreating(modelBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<IdentityUser>().ToTable("user");
			modelBuilder.Entity<ApplicationUser>().ToTable("user");

			modelBuilder.Entity<IdentityRole>().ToTable("role");
			modelBuilder.Entity<IdentityUserRole>().ToTable("userrole");
			modelBuilder.Entity<IdentityUserClaim>().ToTable("userclaim");
			modelBuilder.Entity<IdentityUserLogin>().ToTable("userlogin");
		}

		public override int SaveChanges()
		{
			Audit();
			return base.SaveChanges();
		}

		public async Task<int> SaveChangesAsync()
		{
			Audit();
			return await base.SaveChangesAsync();
		}

		private void Audit()
		{
			var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Added)
				{
					((BaseEntity)entry.Entity).DateCreated = DateTime.UtcNow;
				}
				((BaseEntity)entry.Entity).DateUpdated = DateTime.UtcNow;
			}
		}
	}
}
