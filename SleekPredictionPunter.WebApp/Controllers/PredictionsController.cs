using Microsoft.AspNetCore.Authorization;
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
using SleekPredictionPunter.AppService.Subscriptions;

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
        private readonly ISubscriptionAppService _subscriptionSerivce;
        private readonly ICustomCategoryService _customCategoryService;

        public PredictionsController(IPredictionService predictionService,
			IPricingPlanAppService pricingPlanAppService,
            ISubscriptionAppService subscriptionAppService,
			IPackageAppService packageService,
            IClubService clubService,
            ICategoryService categoryService,
            IPredictorService predictorService,
            IMatchCategoryService matchCategoryService,
            ICustomCategoryService customCategoryService)
        { 
            _predictionService = predictionService;
            _subscriptionSerivce = subscriptionAppService;
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
            Func<Prediction, DateTime> orderByDesc = (s => s.DateCreated);
            return View(await _predictionService.GetPredictionsOrdered(orderDescFunc:orderByDesc,startIndex:0,count:50));
        } 
		public async Task<IActionResult> FrontEndIndex()
        {
			base.AddLinkScriptforPackageSetter(true);
            var plans = await _pricingPlanservice.GetAllPlans();
            var geteFreePlan = plans.FirstOrDefault(c => c.Price < 1);
            Func<Prediction, bool> paidPredicate = null;

            // get to know who is signed in 
            var isAdmin = base.IsAdmin();
            if (isAdmin)
            {
                paidPredicate = (p => p.PricingPlanId != geteFreePlan.Id);
            }
            else
            {
                var useremail = await base.GetUserName();
                Func<Subcription, bool> subFunc = (s => s.SubscriberUsername == useremail);

                var subscriptions = await _subscriptionSerivce.GetAll(subFunc);
                if(subscriptions != null)
                {
                    foreach (var item in subscriptions)
                    {
                        paidPredicate += (x => x.PricingPlanId == item.PricingPlanId);
                    }
                }
            }

            if (paidPredicate != null)
                ViewBag.PaidTips = await _predictionService.GetPredictions(paidPredicate, startIndex: 0, count: 50);

            if (geteFreePlan != null)
            {
                Func<Prediction, bool> freePredicate = (p => p.PricingPlanId == geteFreePlan.Id);
                
                ViewBag.FreeTips = await _predictionService.GetPredictions(freePredicate, startIndex: 0, count: 5);
               
            }

            IEnumerable<IGrouping<long,Prediction>> groupedTipsByPredicationCategories = await _predictionService.ReturnRelationalData(paidPredicate, groupByPredicateCategory:true);

            ViewBag.GrouppedPredictionCategoryList = groupedTipsByPredicationCategories;
             
            var groupedTipsByMatchCategories = await _predictionService.ReturnRelationalData(paidPredicate, groupByMatchCategory:true);

              var groupedTipsByCustomCategories = await _predictionService.ReturnRelationalData(paidPredicate, groupByCustomCategory:true);


            ViewBag.GroupedTipsByCustomCategories = groupedTipsByCustomCategories;
            ViewBag.GroupedTipsByMatchCategories = groupedTipsByMatchCategories;
            ViewBag.GroupedTipsByPredicationCategories = groupedTipsByPredicationCategories;


            //foreach(var item in groupedTipsByPredicationCategories)
            //{
            //    foreach(var keyvalue in item)
            //    {
                    
            //    }
            //}
            

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


        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _predictionService.GetById(id.Value);
            if (prediction == null)
            {
                return NotFound();
            }
            ViewData["CustomCategoryId"] = new SelectList(await _customCategoryService.GetAllQueryable(), "Id", "Id", prediction.CustomCategoryId);
            ViewData["MatchCategoryId"] = new SelectList(await _matchCategoryService.GetAllQueryable(), "Id", "Id", prediction.MatchCategoryId);
            ViewData["PredictionCategoryId"] = new SelectList(await _categoryService.GetCategories(), "Id", "Id", prediction.PredictionCategoryId);
            //ViewData["PredictorId"] = new SelectList(_context.Predictors, "Id", "Id", prediction.PredictorId);
            ViewData["PricingPlanId"] = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "Id", prediction.PricingPlanId);
            return View(prediction);
        }

        // POST: Predictions1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PredictorUserName,ClubA,ClubAOdd,ClubALogoPath,ClubB,ClubBOdd,ClubBLogoPath,PredictionValue,TimeofFixture,PredictorId,CustomCategoryId,MatchCategoryId,PredictionCategoryId,PricingPlanId,ClubAScore,ClubBScore,Id,DateCreated,EntityStatus,DateUpdated")] Prediction prediction)
        {
            if (id != prediction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _predictionService.Update(prediction);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await PredictionExists(prediction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomCategoryId"] = new SelectList(await _customCategoryService.GetAllQueryable(), "Id", "Id", prediction.CustomCategoryId);
            ViewData["MatchCategoryId"] = new SelectList(await _matchCategoryService.GetAllQueryable(), "Id", "Id", prediction.MatchCategoryId);
            ViewData["PredictionCategoryId"] = new SelectList(await _categoryService.GetCategories(), "Id", "Id", prediction.PredictionCategoryId);
            //ViewData["PredictorId"] = new SelectList(_context.Predictors, "Id", "Id", prediction.PredictorId);
            ViewData["PricingPlanId"] = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "Id", prediction.PricingPlanId);
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
            var checkIfExist = await  _predictionService.GetPredictions(e => e.Id == id);
            return checkIfExist != null ? true : false;
        }
    }

	public class PredictionViewModel
	{
		public IEnumerable<Prediction> Predictions  { get; set; }

		public IEnumerable<IDictionary<string,IEnumerable<Prediction>>> GrouppedPredictions { get; set; }

	}
}
