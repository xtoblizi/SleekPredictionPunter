using System;
using Microsoft.Extensions.DependencyInjection;
using SleekPredictionPunter.AppService.AdvertPlacements;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.AppService.BetCategories;
using SleekPredictionPunter.AppService.BetPlatforms;
using SleekPredictionPunter.AppService.BookingCodes;
using SleekPredictionPunter.AppService.Clubs;
using SleekPredictionPunter.AppService.Contacts;
using SleekPredictionPunter.AppService.CustomCategory;
using SleekPredictionPunter.AppService.HomeWiningPlanPreview;
using SleekPredictionPunter.AppService.MatchCategories;
using SleekPredictionPunter.AppService.Matches;
using SleekPredictionPunter.AppService.Packages;
using SleekPredictionPunter.AppService.PaymentService;
using SleekPredictionPunter.AppService.Plans;
//using SleekPredictionPunter.AppService.Packages;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.PredictionCategoryService;
using SleekPredictionPunter.AppService.PredictionMatchMaps;
using SleekPredictionPunter.AppService.PredictionsBookings;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.AppService.Subscriptions;
using SleekPredictionPunter.AppService.ThirdPartyAppService;
using SleekPredictionPunter.AppService.TransactionLog;
using SleekPredictionPunter.AppService.UserManagement;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.Repository;

namespace SleekPredictionPunter.AppService
{
	public static class ServiceCollectionExtension
	{
		public static void AddPredictionApplicationServices(this IServiceCollection services)
		{
			services.AddTransient<ISubscriberService, SubscriberService>();
			services.AddTransient<IPredictionService, PredictionService>();
			services.AddTransient<IAgentService, AgentService>();
			services.AddTransient<IPredictorService, PredictorService>();
			services.AddTransient<IContactAppService, ContactAppService>();
			services.AddTransient<IPackageAppService, PackageAppService>();
			services.AddTransient<IPricingPlanAppService, PricingPlanAppService>();
			services.AddTransient<IThirdPartyUsersAppService, ThirdPartyUsersAppService>();
			services.AddTransient<ICategoryService, CategoryService>();
			services.AddTransient<IAgentRefereeMapService, AgentRefereeMapService>();
			services.AddTransient<IClubService, ClubService>();
            services.AddTransient<IWalletAppService, WalletAppService>();
            services.AddTransient<IPaymentAppService, PaymentAppService>();
			services.AddTransient<ICustomCategoryService, CustomCategoryService>();
			services.AddTransient<IMatchCategoryService, MatchCategoryService>();
			services.AddTransient<ISubscriptionAppService, SubscriptionAppService>();
            services.AddTransient<ITransactionLogAppService, TransactionLogAppService>();
            services.AddTransient<IUserManagementAppService, UserManagementAppService>();

			services.AddTransient<IMatchService, MatchService>();
			services.AddTransient<IPredicationMatchMapService, PredictionMatchMapService>();
			services.AddTransient<IBetCategoryService, BetCategoryService>();
			services.AddTransient<IWiningPlanPreviewService, WiningPlanPreviewService>();
			services.AddTransient<IBetPlatformService, BetPlatformService>();
			services.AddTransient<IPredictionBookingService, PredictionBookingService>();
			services.AddTransient<IBookingCodeService, BookingCodeService>();
			services.AddTransient<IAdvertPlacementService, AdvertPlacementService>();

            // repository DI registration
            services.AddPredictionRepositories();
		}
	}
}
