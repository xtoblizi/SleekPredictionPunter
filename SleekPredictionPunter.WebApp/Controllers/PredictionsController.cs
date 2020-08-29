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
using SleekPredictionPunter.AppService.Matches;
using SleekPredictionPunter.AppService.BetCategories;

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
        private readonly IMatchService _matchService;
        private readonly IBetCategoryService _betCategoryService;
        private readonly IMatchCategoryService _matchCategoryService;
        private readonly ISubscriptionAppService _subscriptionSerivce;
        private readonly ICustomCategoryService _customCategoryService;

        public PredictionsController(IPredictionService predictionService,
			IPricingPlanAppService pricingPlanAppService,
            ISubscriptionAppService subscriptionAppService,
			IPackageAppService packageService,
            IClubService clubService,
            IBetCategoryService betCategoryService,
            IMatchService  matchService,
            ICategoryService categoryService,
            IPredictorService predictorService,
            IMatchCategoryService matchCategoryService,
            ICustomCategoryService customCategoryService)
        { 
            _predictionService = predictionService;
            _subscriptionSerivce = subscriptionAppService;
            _packageService = packageService;
            _betCategoryService = betCategoryService;
            _matchService = matchService;
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
            var result = await _predictionService.GetPredictionsOrdered(orderDescFunc:orderByDesc,startIndex:0,count:50);
            return View(result);
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
            ViewBag.BetCategoryId = new SelectList(await _betCategoryService.GetAllQueryable(null,(x=>x.DateCreated),0,100), "Id", "GetNameAndDescription");
            ViewBag.PredictionCategoryId = new SelectList(await _categoryService.GetCategories(null, (x => x.DateCreated), 0, 100), "Id", "GetNameAndDescription");
            var plan = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "PlanName");
            ViewBag.MatchId = new SelectList(await _matchService.GetMatches(null,(x=>x.DateCreated),0,100), "Id", "GetTeamAvsTeamB");
           
            ViewBag.PricingPlanId = plan;
            return View();
        }

        // POST: Predictions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prediction prediction)
        {
            #region Validate 
            string errorMessage = string.Empty;
            var match = await _matchService.GetMatchById(prediction.MatchId);
            if (match.TimeofMatch <= DateTime.Now.AddMinutes(1))
            {
                errorMessage = "The current time id cannot be greater than 1 iminutes to the time of the selected match";
            }
            
            if (!ModelState.IsValid)
            {
                errorMessage = "You have not properly filled the entries of the prediction form. Verify and try again or contact administrator";
            }
    
            Func<Prediction, bool> validateFunc = (p => p.BetCategoryId == prediction.BetCategoryId
             && p.MatchId == prediction.MatchId
             && p.IsCorrectScore == false);

            var check = await _predictionService.GetFirstOrDefault(validateFunc);
            if (check != null)
            {
                errorMessage = "There is already a prediction for this match on this bet category and its not a correct score";
            }
       
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.PackageId = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "PlanName");
                ViewBag.PredictionCategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "GetNameAndDescription");
                var plan = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "PlanName");
                ViewBag.BetCategoryId = new SelectList(await _betCategoryService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "GetNameAndDescription");
                ViewBag.MatchId = new SelectList(await _matchService.GetMatches(null, (x => x.DateCreated), 0, 100), "Id", "GetTeamAvsTeamB");
                ViewBag.PricingPlanId = plan;

                ViewData["ErrorMessage"] = errorMessage;
                return View(prediction);
            }

            #endregion
            ViewBag.Predictions = "active";
            var getPredictor = await _predictorService.GetByUserName(User.Identity.Name); 
            var pricingPlan = await _pricingPlanservice.GetById(prediction.PricingPlanId);
            var betCategory = await _betCategoryService.GetById(prediction.BetCategoryId);
            var getcategory = await _categoryService.GetCategoryById(prediction.PredictionCategoryId);

          

			prediction.PredictionValue = prediction.PredictionValue;
            prediction.PredictorUserName = User.Identity.Name;
            prediction.TimeofFixture = match.TimeofMatch;
            prediction.BeCategory = betCategory.BetCategoryName;
            prediction.BetCategoryId = betCategory.Id;
            prediction.CustomCategoryId = match.CustomCategoryId;
            prediction.MatchCategoryId = match.MatchCategoryId;
            prediction.MatchCategory = match.MatchCategory;
            prediction.PredictorId = getPredictor.Id;
            prediction.PredictionValue = prediction.PredictionValue;
            prediction.ClubA = match.ClubA;
            prediction.ClubALogoPath = match.ClubALogoPath;
            prediction.ClubB = match.ClubB;
            prediction.ClubBLogoPath = match.ClubBLogoPath;
            prediction.PredictionResult = Model.Enums.PredictionResultEnum.MatchPending;

        
            await _predictionService.InsertPrediction(prediction);
            return RedirectToAction(nameof(Index));
            
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
            ViewData["CustomCategoryId"] = new SelectList(await _customCategoryService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "Id", prediction.CustomCategoryId);
            ViewData["MatchCategoryId"] = new SelectList(await _matchCategoryService.GetAllQueryable(null,(x=>x.DateCreated),0,100), "Id", "Id", prediction.MatchCategoryId);
            ViewData["PredictionCategoryId"] = new SelectList(await _categoryService.GetCategories(null, (x => x.DateCreated), 0, 100), "Id", "Id", prediction.PredictionCategoryId);
            //ViewData["PredictorId"] = new SelectList(_context.Predictors, "Id", "Id", prediction.PredictorId);
            ViewData["PricingPlanId"] = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "Id", prediction.PricingPlanId);
            return View(prediction);
        }

        // POST: Predictions1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PredictorUserName,ClubA,ClubAOdd," +
            "ClubALogoPath,ClubB,ClubBOdd,ClubBLogoPath,PredictionValue," +
            "TimeofFixture,PredictorId,CustomCategoryId,MatchCategoryId," +
            "PredictionCategoryId,PricingPlanId,ClubAScore,ClubBScore,Id," +
            "DateCreated,EntityStatus,DateUpdated")] Prediction prediction)
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
