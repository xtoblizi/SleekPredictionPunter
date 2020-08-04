using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SleekPredictionPunter.AppService.Contacts;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.WebApp.Models;

namespace SleekPredictionPunter.WebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

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
		public IActionResult Contact()
		{
			ViewBag.IsBanner = false;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Contact(ContactDto contactDto)
		{
			if (contactDto == null)
				return View("Contacts details cannot be empty");

			var result = await _contactService.Insert(contactDto);
			return View("Contact succesfully saved");
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
