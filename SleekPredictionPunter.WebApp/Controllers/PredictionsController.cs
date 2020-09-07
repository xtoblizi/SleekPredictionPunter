using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SleekPredictionPunter.AppService.BetCategories;
using SleekPredictionPunter.AppService.Clubs;
using SleekPredictionPunter.AppService.CustomCategory;
using SleekPredictionPunter.AppService.MatchCategories;
using SleekPredictionPunter.AppService.Matches;
using SleekPredictionPunter.AppService.Packages;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.PredictionCategoryService;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.AppService.Subscriptions;
using SleekPredictionPunter.GeneralUtilsAndServices;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        //[Authorize(Roles = "Super Admin")]
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

                IEnumerable<Subcription> subscriptions = await _subscriptionSerivce.GetAll(subFunc);
                if(subscriptions.Any())
                {
                    foreach (var item in subscriptions)
                    {
                        paidPredicate += (x => x.PricingPlanId == item.PricingPlanId );
                    }
                }
            }
            if (geteFreePlan != null)
            {
                Func<Prediction, bool> freePredicate = (p => p.PricingPlanId == geteFreePlan.Id);

                ViewBag.FreeTips = await _predictionService.GetPredictions(freePredicate, startIndex: 0, count: 5);

            }
            if (paidPredicate != null)
            {
                var paidTips = await _predictionService.GetPredictions(paidPredicate, startIndex: 0, count: 100);
                Func<Prediction, DateTime> orderByFunc = (x => x.DateCreated);
                var count = 100;
                ViewBag.PaidTips = paidTips;

                IEnumerable<IGrouping<long, Prediction>> groupedTipsByPredicationCategories = 
                    await _predictionService.ReturnRelationalData(predicate:paidPredicate, orderByFunc: orderByFunc,
                    groupByPredicateCategory: true,startIndex:0,count:count);
                var groupedTipsByMatchCategories = await _predictionService.ReturnRelationalData(predicate: paidPredicate,
                    orderByFunc: orderByFunc,groupByMatchCategory: true,startIndex:0,count:count);
                var groupedTipsByCustomCategories = await _predictionService.ReturnRelationalData(predicate:paidPredicate,orderByFunc:orderByFunc,
                    groupByCustomCategory: true,startIndex:0,count:100);
                var groupTipsByBetCategory = await _predictionService.ReturnRelationalData(predicate:paidPredicate,orderByFunc:orderByFunc,
                    groupByBetCategory: true,startIndex:0,count:count);


                ViewBag.GroupedTipsByCustomCategories = groupedTipsByCustomCategories;
                ViewBag.GroupedTipsByMatchCategories = groupedTipsByMatchCategories;
                ViewBag.GroupedTipsByPredicationCategories = groupedTipsByPredicationCategories;
                ViewBag.GroupedTipsByBetCategories = groupTipsByBetCategory;

            }
            //foreach (var collectionGroup in groupTipsByBetCategory)
            //{
            //    foreach (var item in collectionGroup)
            //    {
            //        item
            //    }

            //}



            //foreach (var item in groupedTipsByPredicationCategories)
            //{
            //    foreach (var keyvalue in item)
            //    {

            //    }
            //}


            return View();
        }

        // GET: Predictions/Details/5
        //[Authorize(Roles = nameof(RoleEnum.SuperAdmin))]
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
        //[Authorize(Roles = nameof(RoleEnum.SuperAdmin))]
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
        // 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prediction prediction)
        {
            #region Validate 
            string errorMessage = string.Empty;
            var match = await _matchService.GetMatchById(prediction.MatchId);
            if (match.TimeofMatch <= DateTime.Now.AddMinutes(10))
            {
                errorMessage = "The current time cannot be greater than 10 iminutes to the time of the selected match";
            }

            if (!ModelState.IsValid)
            {
                errorMessage = "You have not properly filled the entries of the prediction form. Verify and try again or contact administrator";
            }

            Func<Prediction, bool> validateFunc = (p => p.BetCategoryId == prediction.BetCategoryId
             && p.MatchId == prediction.MatchId
             && p.PricingPlanId == prediction.PricingPlanId
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
            var oddCategory = await _categoryService.GetCategoryById(prediction.PredictionCategoryId);

            prediction.PredictionCategoryName = oddCategory.CategoryName;
            prediction.PredictionValue = !string.IsNullOrEmpty(prediction.PredictionValue)? prediction.PredictionValue : oddCategory.CategoryName;
            prediction.PredictorUserName = User.Identity.Name;
            prediction.TimeofFixture = match.TimeofMatch;
            prediction.BeCategory = betCategory.BetCategoryName;
            prediction.BetCategoryId = betCategory.Id;
            prediction.CustomCategoryId = match.CustomCategoryId;
            prediction.MatchCategoryId = match.MatchCategoryId;
            prediction.MatchCategory = match.MatchCategory;
            prediction.PredictorId = getPredictor.Id;
            prediction.PredictionResult = Model.Enums.PredictionResultEnum.MatchPending;
            prediction.ClubA = match.ClubA;
            prediction.ClubB = match.ClubB;
            prediction.ClubALogoPath = match.ClubALogoPath;
            prediction.ClubBLogoPath = match.ClubBLogoPath;
            prediction.PricingPlanName = pricingPlan.PlanName;


            if (ModelState.IsValid)
            {
                await _predictionService.InsertPrediction(prediction);
                return RedirectToAction(nameof(Index));
            }

            return View(prediction);
        }
        [HttpGet]
        //[Authorize(Roles = nameof(RoleEnum.SuperAdmin))]
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
            var rolesEnumList = EnumHelper.GetEnumResults<PredictionResultEnum>();
            ViewBag.MatchStatus = new SelectList(rolesEnumList, "Id", "Name", (int)prediction.PredictionResult);
            ViewBag.MatchId = new SelectList(await _matchService.GetMatches(null, (x => x.DateCreated), 0, 100), "Id", "GetTeamAvsTeamB", prediction.MatchId);
            // sport category
            ViewBag.CustomCategoryId = new SelectList(await _customCategoryService.GetAllQueryable(null, (x => x.DateCreated), 0, 50), "Id", "CategoryName", prediction.CustomCategoryId);
            ViewBag.MatchCategoryId = new SelectList(await _matchCategoryService.GetAllQueryable(null,(x=>x.DateCreated),0,50), "Id", "CategoryName", prediction.MatchCategoryId);
            ViewBag.PredictionCategoryId = new SelectList(await _categoryService.GetCategories(null, (x => x.DateCreated), 0, 50), "Id", "CategoryName", prediction.PredictionCategoryId);
            ViewBag.PricingPlanId = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "PlanName", prediction.PricingPlanId);
            ViewBag.BetCategoryId = new SelectList(await _betCategoryService.GetAllQueryable(), "Id", "BetCategoryName", prediction.BetCategoryId);
           
            return View(prediction);
        }

        // POST: Predictions1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Prediction prediction)
        {
            var match = await _matchService.GetMatchById(prediction.MatchId);
            if (id != prediction.Id)
            {
                return NotFound();
            }

            if (match == null)
                ViewData["TempMessage"] = "The Match has been removed or deleted and cannot be found";

            #region Validation 

            //if ((DateTime.Now < match.TimeofMatch.AddMinutes(100)) && (prediction.PredictionResult ==
            //   PredictionResultEnum.PredictionWon || prediction.PredictionResult == PredictionResultEnum.PredictionLost))
            //{
            //    ViewData["TempMessage"] = "You can not update a match prediction before its calculated play time being 90minutes plus";

            //}

            //if (ViewData["TempMessage"] != null)
            //{
            //    ViewData["TempMessage"] = "The Time of the match and the status conflict";
            //    return View(match);
            //}

            #endregion
            var pred = await _predictionService.GetById(id);

            if (ModelState.IsValid)
            {
                var betcategory = await _betCategoryService.GetById(prediction.BetCategoryId);
                var oddCategory = await _categoryService.GetCategoryById(prediction.PredictionCategoryId);

                pred.PredictionCategoryName = oddCategory.CategoryName;
                pred.PredictionValue = !string.IsNullOrEmpty(prediction.PredictionValue) ? prediction.PredictionValue : oddCategory.CategoryName;
                pred.BeCategory = betcategory?.BetCategoryName;
                pred.BetCategoryId = betcategory.Id;
                pred.PredictionCategoryId = oddCategory.Id;
                pred.PredictionResult = prediction.PredictionResult;
                pred.ClubAScore = prediction.ClubAScore;
                pred.ClubBScore = prediction.ClubBScore;

                await _predictionService.Update(pred);

				#region Update Match As Well

                if(pred.PredictionResult == PredictionResultEnum.PredictionLost
                    || pred.PredictionResult == PredictionResultEnum.PredictionLost)
                {
                    match.MatchStatus = Model.Matches.MatchStatusEnum.Past;
                }else if(pred.PredictionResult == PredictionResultEnum.MatchCancelled)
                {
                    match.MatchStatus = Model.Matches.MatchStatusEnum.Cancelled;
                }else if (pred.PredictionResult == PredictionResultEnum.MatchPostponed)
                {
                    match.MatchStatus = Model.Matches.MatchStatusEnum.Postponed;
                }
                else { }

                await _matchService.Update(match);               

				#endregion

				TempData["TempMessage"] = "Successfully Update the Prediction Details/Result";
                return RedirectToAction(nameof(Index));
            }
            
            var matchstatusList = EnumHelper.GetEnumResults<PredictionResultEnum>();
            ViewBag.MatchStatus = new SelectList(matchstatusList, "Id", "Name", (int)pred.PredictionResult);
            ViewBag.MatchId = new SelectList(await _matchService.GetMatches(null, (x => x.DateCreated), 0, 50), "Id", "GetTeamAvsTeamB", prediction.MatchId);
            ViewBag.CustomCategoryId = new SelectList(await _customCategoryService.GetAllQueryable(null, (x => x.DateCreated), 0, 50), "Id", "Id", prediction.CustomCategoryId);
            ViewBag.MatchCategoryId = new SelectList(await _matchCategoryService.GetAllQueryable(null, (x => x.DateCreated), 0, 50), "Id", "Id", prediction.MatchCategoryId);
            ViewBag.PredictionCategoryId = new SelectList(await _categoryService.GetCategories(null, (x => x.DateCreated), 0, 50), "Id", "Id", prediction.PredictionCategoryId);
            ViewBag.PricingPlanId = new SelectList(await _pricingPlanservice.GetAllPlans(), "Id", "Id", prediction.PricingPlanId);
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

        public async Task<IActionResult> PredictionResults()
        {
            ViewBag.PredictionResults = "active";
            var username = User.Identity.Name;
            var getAllPrediction = await _predictionService.PredictionResult(username: username);

            return View(getAllPrediction);
        }
    }

	public class PredictionViewModel
	{
		public IEnumerable<Prediction> Predictions  { get; set; }

		public IEnumerable<IDictionary<string,IEnumerable<Prediction>>> GrouppedPredictions { get; set; }

	}
}
