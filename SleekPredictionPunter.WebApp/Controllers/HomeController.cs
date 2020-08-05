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
	public class HomeController : BaseController
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IContactAppService _contactService;
		private readonly IPredictionService _predictionService;
		private readonly IPredictorService _predictorService;
		public HomeController(ILogger<HomeController> logger,
			IPredictorService predictorService,IPredictionService predictionService,
			IContactAppService contactAppService)
		{
			_contactService = contactAppService;
			_predictionService = predictionService;
			_predictorService = predictorService;
			_logger = logger;
		}

        public async Task<IActionResult> Index()
		{
			ViewBag.IsBanner = true;
			var gatePredictions = await _predictionService.GetPredictions();
			ViewBag.Predictions = gatePredictions;
			return View();
		}

		[HttpGet("contact")]
		public IActionResult Contact()
		{
			ViewBag.IsBanner = false;
			return View();
		}

		[HttpGet("error")]
		public IActionResult Error(string exceptionMessage = null)
		{
			if (!string.IsNullOrEmpty(exceptionMessage))
			{
				ViewBag.ErrorMessage = exceptionMessage;
				return View(ViewBag.ErrorMessage);
			}

			return View();
			
		}	
		
		[HttpGet("error")]
		public IActionResult AdminHome(string exceptionMessage = null)
		{
			if (!string.IsNullOrEmpty(exceptionMessage))
			{
				ViewBag.ErrorMessage = exceptionMessage;
				return View(ViewBag.ErrorMessage);
			}

			return View();
			
		}

		[HttpPost]
		public async Task<IActionResult> Contact(ContactDto contactDto)
		{
			if (contactDto == null)
			{
				ViewBag.Error = "Contacts details cannot be empty";
				return View(ViewBag.Error);
			}
				

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
