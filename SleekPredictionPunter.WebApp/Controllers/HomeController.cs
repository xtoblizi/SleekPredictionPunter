﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SleekPredictionPunter.AppService.Contacts;
using SleekPredictionPunter.AppService.MatchCategories;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.WebApp.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
	public class HomeController : BaseController
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IContactAppService _contactService;
		private readonly IPredictionService _predictionService;
		private readonly IPredictorService _predictorService;
		private readonly IMatchCategoryService _matchCategoryService;
		private readonly IPricingPlanAppService _pricingPlanservice;
		public HomeController(ILogger<HomeController> logger,
			IPredictorService predictorService,
			IMatchCategoryService matchCategoryService,
			IPredictionService predictionService,
			IContactAppService contactAppService,
			IPricingPlanAppService pricingPlanAppService)
		{
			_contactService = contactAppService;
			_matchCategoryService = matchCategoryService;
			_predictionService = predictionService;
			_predictorService = predictorService;
			_logger = logger;
			_pricingPlanservice = pricingPlanAppService;
		}


		public async Task<IActionResult> Index()
		{
			ViewBag.IsBanner = true;

			// this is used to conditionally show or hide breadcumbanner in the view of the concerned page
			// pass the value true or false in the base method "ShowBreadCumBannerSetter"
			base.ShowBreadCumBannerSetter(true);

			var plans = await _pricingPlanservice.GetAllPlans();
			var geteFreePlan = plans.FirstOrDefault(c => c.Price < 1);
			var matchCategory = await _matchCategoryService.GetAllQueryable(null, (x=>x.DateCreated),0, 10);

			ViewBag.MatchCategory = matchCategory;

			Func<Prediction, bool> freePredicate = null;
			if (geteFreePlan != null)
			{
				freePredicate = (p => p.PricingPlanId == geteFreePlan.Id);
				Func<Prediction, DateTime> orderByDescFunc = (x => x.DateCreated);
				var freePredications =
						await _predictionService.GetPredictionsOrdered(freePredicate, orderByDescFunc, startIndex: 0, count: 100);
			}
			
			#region Predications in groupings

			if (geteFreePlan != null)
			{
				Func<Prediction, DateTime> orderByDescFunc = (x => x.DateCreated);
				freePredicate = (p => p.PricingPlanId == geteFreePlan.Id);
				ViewBag.FreeTips = await _predictionService.GetPredictionsOrdered(freePredicate, orderByDescFunc,startIndex: 0, count: 10);
			}

			Func<Prediction, DateTime> orderByFunc = (x => x.DateCreated);

			var groupedTipsByPredicationCategories = await _predictionService.ReturnRelationalData(predicate:freePredicate,
				orderByFunc:orderByFunc,
				groupByPredicateCategory: true);

			var groupedTipsByMatchCategories = await _predictionService.ReturnRelationalData(predicate: freePredicate,
				orderByFunc:orderByFunc,
				groupByMatchCategory: true);

			var groupedTipsByCustomCategories = await _predictionService.ReturnRelationalData(predicate:freePredicate,
				orderByFunc:orderByFunc,
				groupByCustomCategory: true);


			ViewBag.GroupedTipsByCustomCategories = groupedTipsByCustomCategories;
			ViewBag.GroupedTipsByMatchCategories = groupedTipsByMatchCategories;
			ViewBag.GroupedTipsByPredicationCategories = groupedTipsByPredicationCategories;
			#endregion
			

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

		
		public IActionResult Contact(string message = null)
		{
			ViewBag.IsBanner = false;
            if (string.IsNullOrEmpty(message))
            {
				return View(message);
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Contact(ContactDto contactDto)
		{
			if (contactDto == null)
			{
				TempData["ContactCreation"] = "Contacts details cannot be empty";
				return View(TempData["ContactCreation"]);
			}

			var result = await _contactService.Insert(contactDto);
			TempData["ContactCreation"] = "Your contact request has successfully been received and your would be responded to in less than 48hours. Thanks for your reach : Predictive Power";
			return View();
		}

		/// <summary>
		/// This is the view for the admin to view all contacts tickets
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> ContactIndex()
		{
			ViewBag.IsBanner = false;
			ViewBag.Contact = "active";
			var result = await _contactService.GetAllQueryable();
			return View(result);
		}

		/// <summary>
		/// this is the home page of the admin
		/// </summary>
		/// <param name="exceptionMessage"></param>
		/// <returns></returns>
		public IActionResult AdminHome(string exceptionMessage = null)
		{
			if (!string.IsNullOrEmpty(exceptionMessage))
			{
				ViewBag.ErrorMessage = exceptionMessage;
				return View(ViewBag.ErrorMessage);
			}

			return View();
			
		}
	
		

		public IActionResult Privacy()
		{
			ViewBag.IsBanner = true;
			return View();
		}
		public IActionResult Howtoplay()
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
