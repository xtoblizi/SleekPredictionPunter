using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.IdentityModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.DataInfrastructure
{
	public class PredictionDbFactory : IDesignTimeDbContextFactory<PredictionDbContext>
	{
		const string  connectionstring = "Data Source= (LocalDb)\\MSSQLLocalDB;Initial Catalog = SleekDb; Integrated Security = True; Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=True";
		public PredictionDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<PredictionDbContext>();
			optionsBuilder.UseSqlServer(connectionstring);

			return new PredictionDbContext(optionsBuilder.Options);
		}
	}
	public class PredictionDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
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
		public DbSet<Prediction> Predictions { get; set; }
		public DbSet<Predictor> Predictors { get; set; }


		/// <summary>
		/// Override method on creation of the tables of the database
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder builder)
		{		
			base.OnModelCreating(builder);

			builder.Entity<ApplicationUser>(entity =>
			{
				entity.ToTable(name: "Users");
			});

			builder.Entity<ApplicationRole>(entity =>
			{
				entity.ToTable(name: "Roles");
			});
			builder.Entity<IdentityUserRole<string>>(entity =>
			{
				entity.ToTable("UserRoles");
				//in case you chagned the TKey type
				//  entity.HasKey(key => new { key.UserId, key.RoleId });
			});

			builder.Entity<IdentityUserClaim<string>>(entity =>
			{
				entity.ToTable("UserClaims");
			});

			builder.Entity<IdentityUserLogin<string>>(entity =>
			{
				entity.ToTable("UserLogins");
				//in case you chagned the TKey type
				//  entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });       
			});

			builder.Entity<IdentityRoleClaim<string>>(entity =>
			{
				entity.ToTable("RoleClaims");

			});

			builder.Entity<IdentityUserToken<string>>(entity =>
			{
				entity.ToTable("UserTokens");
				//in case you chagned the TKey type
				// entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });

			});

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
