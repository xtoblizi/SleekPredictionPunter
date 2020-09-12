using Microsoft.AspNetCore.Http;
using SleekPredictionPunter.Model.BettingPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.ViewModels.BetPlatforms
{
	public class BetPlatformVm
	{
		public IFormFile LogoPath { get; set; }
		public string BetPlatformName { get; set; }

		public string Caption { get; set; }
	}
}
