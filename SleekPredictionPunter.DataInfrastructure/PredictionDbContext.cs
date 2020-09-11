using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.PricingPlan;
using SleekPredictionPunter.Model.Wallets;
using SleekPredictionPunter.Model.TransactionLogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using SleekPredictionPunter.Model.Matches;
using SleekPredictionPunter.Model.PredicationMatchMaps;
using SleekPredictionPunter.Model.Categoriess;
using SleekPredictionPunter.Model.HomeDataModels;

namespace SleekPredictionPunter.DataInfrastructure
{
	public class PredictionDbFactory : IDesignTimeDbContextFactory<PredictionDbContext>
	{
        // const string connectionstring = "Data Source=.;Initial Catalog = SleekPredictionPunterDb; Integrated Security = True;" +
        //   " Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=True";

       // const string connectionstring = "Data Source=DESKTOP-JBDM8G2\\SQLEXPRESS;Initial Catalog = SleekPredictionPunterDb; Integrated Security = True;" +
         //   " Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=True";

        const string connectionstring = "Data Source=localhost\\SQLEXPRESS;Initial Catalog = SleekPredictionPunterDb; Integrated Security = True;" +
            " Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=True";
        public PredictionDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<PredictionDbContext>();
			optionsBuilder.UseSqlServer(connectionstring);

			return new PredictionDbContext(optionsBuilder.Options);
		}
	}
	public class PredictionDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
	{
		public PredictionDbContext(DbContextOptions<PredictionDbContext> options)
			: base(options)
		{
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		// add tables of the database as the dbcontext properties here.

		public DbSet<Agent> AgentUsers { get; set; }
		public DbSet<Subscriber> Subscribers { get; set; }
		public DbSet<Subcription> Subcriptions { get; set; }
		public DbSet<AgentRefereeMap> AgentRefereeMaps { get; set; } 
		public DbSet<Prediction> Predictions { get; set; }
		public DbSet<Predictor> Predictors { get; set; }
		public DbSet<ThirdPartyUsersModel> ThirdPartyUsers { get; set; }
        public DbSet<PlanBenefitQuestionsModel> PricePlanQuestions { get; set; }
        public DbSet<PlanPricingBenefitsModel> PricePlanBenefits { get; set; }
        public DbSet<PricingPlanModel> PricePlans { get; set; }
        public DbSet<WalletModel> Wallets { get; set; }
        public DbSet<TransactionLogModel> TransactionLogs { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<PredictionCategory> PredictionCategories { get; set; }
		public DbSet<Club> Clubs { get; set; }
        public DbSet<WalletModel> Wallet { get; set; }
		public DbSet<MatchCategory> MatchCategories { get; set; }
		public DbSet<Match> Matches { get; set; }
		public DbSet<PredictionMatchMap> PredictionMapMaps { get; set; }
		public DbSet<CustomCategory> CustomCategories { get; set; }
		public DbSet<BetCategory> BetCategories { get; set; }
		public DbSet<WinningPlanPreviewSummary> WinningPlanPreviewSummary { get; set; }


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
			builder.Entity<CustomCategory>(entity =>
			{
				entity.ToTable(name: "SportLeagueCategory");
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
			builder.Entity<Prediction>(entity =>
			{
				entity.Property(p => p.MatchId)
					.HasDefaultValue(0);
				entity.Property(p => p.PredictionCategoryName)
					.HasDefaultValue("Not Set");
			});
			builder.Entity<Match>(entity =>
			{
				entity.Ignore(p => p.ReturnStatus);
			});

			builder.Entity<IdentityUserToken<string>>(entity =>
			{
				entity.ToTable("UserTokens");
				//in case you chagned the TKey type
				// entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
			});

			foreach (var property in builder.Model.GetEntityTypes()
				   .SelectMany(t => t.GetProperties())
				   .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
			{
				// EF Core 1 & 2
				//property.Relational().ColumnType = "decimal(18, 6)";

				// EF Core 3
				property.SetColumnType("decimal(18, 2)");

				// EF Core 5
				//property.SetPrecision(18);
				//property.SetScale(6);
			}

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
			var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity
			&& (x.State == EntityState.Added || x.State == EntityState.Modified));
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
