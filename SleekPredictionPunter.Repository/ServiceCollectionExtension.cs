using Microsoft.Extensions.DependencyInjection;
using SleekPredictionPunter.Repository.Base;
using System;

namespace SleekPredictionPunter.Repository
{
	public static class ServiceCollectionExtension
	{
		public static void AddPredictionRepositories(this IServiceCollection service)
        {
			service.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
		}
	}
}
