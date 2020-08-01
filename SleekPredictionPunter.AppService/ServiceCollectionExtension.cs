using System;
using Microsoft.Extensions.DependencyInjection;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.AppService.PredictionAppService;
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

			// repository DI registration
			services.AddPredictionRepositories();
		}
	}
}
