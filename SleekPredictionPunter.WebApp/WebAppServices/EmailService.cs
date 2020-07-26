using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.WebAppServices
{
	public class EmailService : IEmailSender
	{
		public EmailService()
		{
			
		}
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			// message would be sent here
		}
	}
}
