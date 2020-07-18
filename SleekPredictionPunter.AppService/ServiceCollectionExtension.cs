using System;
using Microsoft.Extensions.DependencyInjection;
using SleekPredictionPunter.Repository;

namespace SleekPredictionPunter.AppService
{
	public static class ServiceCollectionExtension
	{
		public static void AddPredictionApplicationServices(this IServiceCollection services)
		{
			services.AddTransient<ISubscriberService, SubscriberService>();

			// repository DI registration
			services.AddPredictionRepositories();
		}
	}
}
