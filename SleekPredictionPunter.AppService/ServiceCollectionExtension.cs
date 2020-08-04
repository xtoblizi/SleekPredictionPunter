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
			services.AddTransient<IContactAppService, ContactAppService>();

			// repository DI registration
			services.AddPredictionRepositories();
		}
	}
}
