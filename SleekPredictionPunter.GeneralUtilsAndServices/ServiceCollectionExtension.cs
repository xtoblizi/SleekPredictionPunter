using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.GeneralUtilsAndServices
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddGeneralUtilServices(this IServiceCollection services)
		{
			services.AddTransient<IFileHelper, FileHelper>();

			return services;
		}
	}
}
