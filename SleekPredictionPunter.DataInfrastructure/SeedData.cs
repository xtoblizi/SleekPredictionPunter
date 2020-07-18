using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SleekPredictionPunter.GeneralUtilsAndServices;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.DataInfrastructure
{
	public static class SeedData
	{
		public async static Task Initialize(IServiceProvider serviceProvider)
		{
			try
			{
				var context = serviceProvider.GetRequiredService<PredictionDbContext>();
				var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				context.Database.EnsureCreated();
				ApplicationUser user = null;
				bool isUserCreated = false;

				if (!context.Users.Any(x => x.UserName.Equals("systemadmin")))
				{
					user = new ApplicationUser()
					{
						Email = $"systemadmin@totalprediction.com",
						SecurityStamp = Guid.NewGuid().ToString(),
						UserName = "systemadmin",
						LastName = "Admin",
						FirstName = "Total Prediction Admin",
						Status = UserStatusEnum.Activated,
						EmailConfirmed = true,
						TwoFactorEnabled = false,
						PhoneNumberConfirmed = true
					};
					await userManager.CreateAsync(user, "password");
					isUserCreated = true;
				}

				// Seed Roles data from RolesEnum to Roles table 	
				#region Create roles
				var rolesEnumList = EnumHelper.GetEnumResults<RoleEnum>();
				if (rolesEnumList.Any())
				{
					foreach (var item in rolesEnumList)
					{
						var roleRecord = context.Roles.Where(x => x.Name.Equals(item.Name));
						if (roleRecord.FirstOrDefault()?.Name == null)
						{
							IdentityRole role = new IdentityRole()
							{
								ConcurrencyStamp = Guid.NewGuid().ToString(),
								Name = item.Name,
							};
							await roleManager.CreateAsync(role);
						}
					}
					var systemAdminrole = RoleEnum.SystemAdmin.GetDescription();
					if (isUserCreated == true && !(await userManager.IsInRoleAsync(user, systemAdminrole)))
					{
						await userManager.AddToRoleAsync(user, RoleEnum.SystemAdmin.GetDescription());
					}
				}

				#endregion
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
