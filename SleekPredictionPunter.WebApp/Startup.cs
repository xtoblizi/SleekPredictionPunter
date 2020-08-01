using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.DataInfrastructure;
using System.IO;

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

			services.AddDbContext<PredictionDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});
	

			services.AddUserIdentityServices();
			services.AddPredictionApplicationServices();
			services.AddRazorPages();
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

			app.UseAuthorization();
			app.UseAuthentication();
			//app.UseMvc();
			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapAreaControllerRoute(
			//			name:"adminarea",
			//			areaName:"admin",
			//			pattern:"{identity}/{controller=Home}/{action=Index}/{id?}");

			//	endpoints.MapAreaControllerRoute(
			//			name: "adminarea",
			//			areaName: "admin",
			//			pattern: "{admin}/{controller=Home}/{action=Index}/{id?}");

			//	endpoints.MapControllerRoute(
			//		name: "default",
			//		pattern: "{controller=Home}/{action=Index}/{id?}");
			//});

			_=SeedData.Initialize(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
