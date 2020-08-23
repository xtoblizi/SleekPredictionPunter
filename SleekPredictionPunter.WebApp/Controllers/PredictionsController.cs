﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SleekPredictionPunter.AppService.Clubs;
using SleekPredictionPunter.AppService.CustomCategory;
using SleekPredictionPunter.AppService.MatchCategories;
using SleekPredictionPunter.AppService.Packages;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.PredictionAppService.Dtos;
using SleekPredictionPunter.AppService.PredictionCategoryService;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 

namespace SleekPredictionPunter.WebApp.Controllers
{
    //[Authorize]
    public class PredictionsController : BaseController
    { 
        private readonly IPredictionService _predictionService;
        private readonly IPackageAppService _packageService;
		private readonly IPricingPlanAppService _pricingPlanservice;
        private readonly IClubService _clubService;
        private readonly ICategoryService _categoryService;
        private readonly IPredictorService _predictorService;
        private readonly IMatchCategoryService _matchCategoryService;
        private readonly ICustomCategoryService _customCategoryService;

        public PredictionsController(IPredictionService predictionService,
			IPricingPlanAppService pricingPlanAppService,
			IPackageAppService packageService,
            IClubService clubService,
            ICategoryService categoryService,
            IPredictorService predictorService,
            IMatchCategoryService matchCategoryService,
            ICustomCategoryService customCategoryService)
        { 
            _predictionService = predictionService;
            _packageService = packageService;
			_pricingPlanservice = pricingPlanAppService;
            _clubService = clubService;
            _categoryService = categoryService;
            _predictorService = predictorService;
            _matchCategoryService = matchCategoryService;
            _customCategoryService = customCategoryService;
        }

        // GET: Predictions
		/// <summary>
		/// This is the admin view page for predictions
		/// </summary>
		/// <returns></returns>
        public async Task<IActionResult> Index()
        {
            ViewBag.Predictions = "active";
            return View(await _predictionService.GetPredictions());
        } 
		public async Task<IActionResult> FrontEndIndex()
        {
			base.AddLinkScriptforPackageSetter(true);
            var plans = await _pricingPlanservice.GetAllPlans();
            var geteFreePlan = plans.FirstOrDefault(c => c.Price < 1);
            
            if(geteFreePlan != null)
            {
                Func<Prediction, bool> freePredicate = (p => p.PricingPlanId == geteFreePlan.Id);
                Func<Prediction, bool> paidPredicate = (p => p.PricingPlanId != geteFreePlan.Id);

                ViewBag.FreeTips = await _predictionService.GetPredictions(freePredicate, startIndex: 0, count: 5);
                ViewBag.PaidTips = await _predictionService.GetPredictions(paidPredicate, startIndex: 0, count: 5); 
            }

            var groupedTipsByPredicationCategories = await _predictionService.ReturnRelationalData(null,groupByPredicateCategory:true);

            ViewBag.GrouppedPredictionCategoryList = groupedTipsByPredicationCategories;
             
            var groupedTipsByMatchCategories = await _predictionService.ReturnRelationalData(null,groupByMatchCategory:true);

              var groupedTipsByCustomCategories = await _predictionService.ReturnRelationalData(null,groupByCustomCategory:true);


            ViewBag.GroupedTipsByCustomCategories = groupedTipsByCustomCategories;
            ViewBag.GroupedTipsByMatchCategories = groupedTipsByMatchCategories;
            ViewBag.GroupedTipsByPredicationCategories = groupedTipsByPredicationCategories;


            foreach(var item in groupedTipsByPredicationCategories)
            {
                foreach(var keyvalue in item)
                {
                    
                }
            }
            

            return View();
        }

        // GET: Predictions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            ViewBag.Predictions = "active";
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _predictionService.GetPredictionByPredictor(id.Value);
            if (prediction == null)
            {
                return NotFound();
            }

            return View(prediction);
        }

        // GET: Predictions/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Predictions = "active";

            ViewBag.PackageId = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "PlanName");
            ViewBag.ClubA = new SelectList(await _clubService.GetAllQueryable(), "ClubName", "ClubName");
            ViewBag.ClubB = new SelectList(await _clubService.GetAllQueryable(), "ClubName", "ClubName");
            ViewBag.PredictionCategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "GetNameAndDescription");
            var plan = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "PlanName");
            ViewBag.MatchCategoryId = new SelectList(await _matchCategoryService.GetAllQueryable(), "Id", "CategoryName");
            ViewBag.CustomCategoryId = new SelectList(await _customCategoryService.GetAllQueryable(), "Id", "CategoryName");
           
            ViewBag.PricingPlanId = plan;
            return View();
        }

        // POST: Predictions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Prediction prediction)
        {
            ViewBag.Predictions = "active";
            var getPredictor = await _predictorService.GetByUserName(User.Identity.Name); 
            var pricingPlan = await _pricingPlanservice.GetById(prediction.PricingPlanId);
            var getcategory = await _categoryService.GetCategoryById(prediction.PredictionCategoryId);



            prediction.DateCreated = DateTime.UtcNow;
            prediction.DateUpdated = DateTime.UtcNow;
            prediction.PredictionValue = prediction.PredictionValue;
            prediction.Predictor = prediction.Predictor;
            prediction.PredictorUserName = User.Identity.Name;
            prediction.TimeofFixture = prediction.TimeofFixture;
            prediction.PricingPlan = pricingPlan;
            prediction.PredictorId = getPredictor.Id;
            prediction.PredictionValue = getcategory.CategoryName;

            if (ModelState.IsValid)
            {
                await _predictionService.InsertPrediction(prediction);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClubA = new SelectList(await _clubService.GetAllQueryable(), "ClubName", "ClubName", prediction.ClubA);
            ViewBag.ClubB = new SelectList(await _clubService.GetAllQueryable(), "ClubName", "ClubName", prediction.ClubB);
            ViewBag.PredictionCategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "GetNameAndDescription", prediction.PredictionValue);
            ViewBag.PricingPlanId = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "PlanName", prediction.PricingPlanId);
            ViewBag.MatchCategoryId = new SelectList(await _matchCategoryService.GetAllQueryable(), "Id", "CategoryName", prediction.MatchCategoryId);
            ViewBag.CustomCategoryId = new SelectList(await _customCategoryService.GetAllQueryable(), "Id", "CategoryName", prediction.CustomCategoryId);
            

            return View(prediction);
        }

    
        // GET: Predictions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            ViewBag.Predictions = "active";
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _predictionService.GetPredictionByPredictor(id.Value);
            if (prediction == null)
            {
                return NotFound();
            }

            return View(prediction);
        }

        // POST: Predictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var prediction = await _predictionService.GetPredictionByPredictor(id);
            await _predictionService.RemovePredictionBy(prediction, true);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PredictionExists(long id)
        {
            var checkIfExist = await  _predictionService.GetPredictions();
            return checkIfExist.Any(e => e.Id == id);
        }
    }

	public class PredictionViewModel
	{
		public IEnumerable<Prediction> Predictions  { get; set; }

		public IEnumerable<IDictionary<string,IEnumerable<Prediction>>> GrouppedPredictions { get; set; }

	}
}
