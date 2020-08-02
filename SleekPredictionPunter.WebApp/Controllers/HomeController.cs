using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.WebApp.Models;

namespace SleekPredictionPunter.WebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IPredictionService _predictionService;
		private readonly IPredictorService _predictorService;

        public HomeController(ILogger<HomeController> logger, IPredictionService predictionService, IPredictorService predictorService)
        {
            _logger = logger;
            _predictionService = predictionService;
            _predictorService = predictorService;
        }

        public async Task<IActionResult> Index()
		{
			ViewBag.IsBanner = true;
			var gatePredictions = await _predictionService.GetPredictions();
			ViewBag.Predictions = gatePredictions;
			return View();
		}

		public IActionResult Privacy()
		{
			ViewBag.IsBanner = true;
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
