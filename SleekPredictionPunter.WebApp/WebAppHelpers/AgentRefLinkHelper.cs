using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.WebAppHelpers
{
	public class AgentRefLinkHelper
	{
		public static string ReturnAgentRefereerCode(string refCode, Microsoft.AspNetCore.Http.HttpRequest request)
		{
			var actionLink = $"Identity/Account/Register?registrationType=1&userType=2&refcode={refCode}";
			var refLink = $"{request.Scheme}://{request.Host}/{actionLink}";

			return refLink;
		}
		
	}
}
