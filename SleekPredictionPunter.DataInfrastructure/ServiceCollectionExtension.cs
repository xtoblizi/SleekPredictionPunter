using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SleekPredictionPunter.Model.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.DataInfrastructure
{
	public static class ServiceCollectionExtension
	{
		public static void AddUserIdentityServices(this IServiceCollection services)
		{

			services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
			{

				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 4;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
			})
				.AddEntityFrameworkStores<PredictionDbContext>()
				.AddDefaultTokenProviders();

			services.AddAuthentication(options =>
			{
				//options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				//options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				//options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					RequireExpirationTime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PredictivePowerSecurityTokens"))
				};
			});

		}
	}
}
