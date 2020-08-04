using System;
using Microsoft.Extensions.DependencyInjection;
using SleekPredictionPunter.AppService.Contacts;
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

			// repository DI registration
			services.AddPredictionRepositories();
		}
	}
}
