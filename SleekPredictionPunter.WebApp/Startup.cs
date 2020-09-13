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

			services.AddDbContext<PredictionDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});

			//services.AddAuthentication(options =>
			//{
			//	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//}).AddCookie(options =>
			//{
			//	options.LoginPath = "/identity/account/login";
			//	options.AccessDeniedPath = "";
			//	options.ExpireTimeSpan = TimeSpan.FromMinutes(10500);
			//});
			services.AddSession(x => { x.IdleTimeout = TimeSpan.FromHours(24); });

			services.AddUserIdentityServices();
			services.AddPredictionApplicationServices();
			services.AddRazorPages();
			services.AddGeneralUtilServices();
			services.AddPaging();

			services.AddAuthentication()
				.AddGoogle(opt =>
				{
					opt.ClientId = "232216909561-l8q4np7q0pd711i5gplqv8fq9aijurla.apps.googleusercontent.com";
					opt.ClientSecret = "rYjORJBnSLg5l51eiynfGkwJ";
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
				endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
