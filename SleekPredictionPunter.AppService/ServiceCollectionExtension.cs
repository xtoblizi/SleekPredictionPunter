using System;
using Microsoft.Extensions.DependencyInjection;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.AppService.Contacts;
//using SleekPredictionPunter.AppService.Packages;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.AppService.ThirdPartyAppService;
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
			services.AddTransient<IThirdPartyUsersAppService, ThirdPartyUsersAppService>();
			//services.AddTransient<IPackageAppService, PackageAppService>();

			// repository DI registration
			services.AddPredictionRepositories();
		}
	}
}
