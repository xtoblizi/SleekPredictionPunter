using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using SleekPredictionPunter.WebApp.WebAppServices;

[assembly: HostingStartup(typeof(SleekPredictionPunter.WebApp.Areas.Identity.IdentityHostingStartup))]
namespace SleekPredictionPunter.WebApp.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
				services.AddTransient<IEmailSender, EmailService>();
			});
        }
    }
}