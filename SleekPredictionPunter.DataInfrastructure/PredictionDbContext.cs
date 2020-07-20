using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.IdentityModels;
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

		public DbSet<Agent> AgentUsers { get; set; }
		public DbSet<Predictor> PredictorUsers { get; set; }
		public DbSet<SubcriberPredictorMap> SubcriberPredictorMaps { get; set; }
		public DbSet<Subscriber> Subscribers { get; set; }
		public DbSet<AgentRefereeMap> AgentRefereeMaps { get; set; }

		/// <summary>
		/// Override method on creation of the tables of the database
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{		
			base.OnModelCreating(modelBuilder);
			
			//modelBuilder.Entity<IdentityUser>().ToTable("user");
			//modelBuilder.Entity<ApplicationUser>().ToTable("user");

			//modelBuilder.Entity<ApplicationRole>().ToTable("role");
			//modelBuilder.Entity<IdentityUserRole>().ToTable("userrole");
			//modelBuilder.Entity<IdentityUserClaim>().ToTable("userclaim");
			//modelBuilder.Entity<IdentityUserLogin>().ToTable("userlogin");
		}


		public override int SaveChanges()
		{
			Audit();
			return base.SaveChanges();
		}

		public async Task<long> SaveChangesAsync()
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
