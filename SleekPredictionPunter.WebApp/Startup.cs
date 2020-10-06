using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReflectionIT.Mvc.Paging;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.GeneralUtilsAndServices;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			 Configuration = new ConfigurationBuilder()
			 .SetBasePath(Directory.GetCurrentDirectory())
			 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
			 .AddEnvironmentVariables()
			 .Build();
		}
		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
			.AddRazorPagesOptions(options =>
			{
				
				options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
				options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
			});

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = $"/Identity/Account/Login";
				options.LogoutPath = $"/Identity/Account/Logout";
				options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
			
			});

			services.AddDbContext<PredictionDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});

			services.AddSession(x => { x.IdleTimeout = TimeSpan.FromHours(24); });

			services.AddUserIdentityServices();
			services.AddPredictionApplicationServices();
			services.AddRazorPages();
			services.AddGeneralUtilServices();

			services.AddAuthentication()
				.AddGoogle(opt =>
				{
					opt.ClientId = "697864622224-qsqgv3d4ija46e2ipis1bmmv4iop3h2c.apps.googleusercontent.com";
					opt.ClientSecret = "plKOkz5r4LwKUd4qD7e37Gws";
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseSession();
			app.UseAuthorization();
			app.UseAuthentication();
			
			_ = SeedData.Initialize(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
			});

			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapControllerRoute(
			//		name: "default",
			//		pattern: "{controller=Home}/{action=Index}/{id?}");
			//	endpoints.MapRazorPages();
			//});
		}
	}
}
