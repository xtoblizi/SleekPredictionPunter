using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.BetCategories;
using SleekPredictionPunter.AppService.BetPlatforms;
using SleekPredictionPunter.AppService.Clubs;
using SleekPredictionPunter.AppService.CustomCategory;
using SleekPredictionPunter.AppService.MatchCategories;
using SleekPredictionPunter.AppService.Matches;
using SleekPredictionPunter.AppService.Packages;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.PredictionCategoryService;
using SleekPredictionPunter.AppService.PredictionsBookings;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.AppService.Subscriptions;
using SleekPredictionPunter.GeneralUtilsAndServices.JsonHelper;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.PredictionBookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class PredictionBookingsController : BaseController
    {
        private readonly IPredictionBookingService _context;
        private readonly IBetPlatformService _betPlatformService;
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

        private static List<Prediction> BookingPredictionsStatic = new List<Prediction>();
        private static List<Prediction> TotalPredictions = new List<Prediction>();

        public PredictionBookingsController(
            IPredictionService predictionService,
            IPricingPlanAppService pricingPlanAppService,
            ISubscriptionAppService subscriptionAppService,
            IPackageAppService packageService,
            IClubService clubService,
            IBetCategoryService betCategoryService,
            IMatchService matchService,
            ICategoryService categoryService,
            IPredictorService predictorService,
            IMatchCategoryService matchCategoryService,
            ICustomCategoryService customCategoryService,
            IPredictionBookingService context, IBetPlatformService betPlatformService)
        {
            _betPlatformService = betPlatformService;
            _context = context;

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

        // GET: PredictionBookings
        public async Task<IActionResult> Index(string search = null, int startIndex = 0 , int count = int.MaxValue)
        {
            ViewBag.BetPlatFormAndCode = "active";

            Func<PredictionBooking, bool> func = (x => x.BookingCodes.Contains(search));
            Func<PredictionBooking, DateTime> orderFunc = (x => x.DateCreated);
            var result = await _context.GetBookings(whereFunc: func, orderFunc, startIndex, count);

            return View(result);
        }

        // GET: PredictionBookings/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            var predictionBooking = await _context.GetBookingById(id.Value);
            ViewBag.BetPlatFormAndCode = "active";

            if (predictionBooking == null)
            {
                return NotFound();
            }

            var listOfPredictions = new List<Prediction>();
            var jarrayOfPredictions = JsonHelper.TryDeSerialize<List<Prediction>>(predictionBooking.Predictions);
            var betplatfromsCodes = JsonHelper.TryDeSerialize<List<PlatformBookingCode>>(predictionBooking.BookingCodeWithRelationToPlatform);

            var bookingDetails = new PredictionBookingDto()
            {
                PlatformBookingCodesList = betplatfromsCodes.Item2,
                PredictionsList = jarrayOfPredictions.Item2,
                BonusCode = predictionBooking.BonusCode,
                Odd = predictionBooking.Odd,
                PredictedBy = predictionBooking.PredictedBy,
                DisplayonHome = predictionBooking.DisplayonHome,
                EntityStatus = predictionBooking.EntityStatus,
                BookingPlatformIds = predictionBooking.BookingPlatformIds,
                LeastMatchstattime = predictionBooking.LeastMatchstattime,
                PricingPlan = predictionBooking.PricingPlan,
                DateCreated = predictionBooking.DateCreated

            };
            return View(bookingDetails);
        }

        // GET: PredictionBookings/Create
        public async Task<IActionResult> Create(string search = null, int startIndex = 0, int count = int.MaxValue)
        {
            ViewBag.BetPlatFormAndCode = "active";

            search = string.IsNullOrEmpty(search) ? string.Empty : search;
            Func<Prediction, bool> func = null;
            if (string.IsNullOrEmpty(search))
            {
               func = (x => x.ClubA.Contains(search) || x.ClubB.Contains(search));
            }

            Func<Prediction, DateTime> orderFunc = (x => x.DateCreated);

            var predictions = await _predictionService.GetPredictionsOrdered(func,orderFunc,startIndex,70);

            if(!TotalPredictions.Any())
                TotalPredictions = predictions.ToList();

            //TotalPredictions.Select(x => BookingPredictionsStatic.Any(y => x.Id != y.Id));

            ViewBag.Predictions = TotalPredictions;
            ViewBag.BookingPredictionsStatic = BookingPredictionsStatic;

            return View();
        }


        // POST: PredictionBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPrediction(long predictionId)
        {
            ViewBag.BetPlatFormAndCode = "active";

            var predictionBooking = await _predictionService.GetById(predictionId);
            BookingPredictionsStatic.Add(predictionBooking);
            TotalPredictions.Remove(predictionBooking);
           // TotalPredictions.Select(x => BookingPredictionsStatic.Any(y => x.Id != y.Id));

            ViewBag.Predictions = TotalPredictions;
            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePrediction(long predictionId)
        {

            var predictionBooking = await _predictionService.GetById(predictionId);
            BookingPredictionsStatic.Remove(predictionBooking);

            TotalPredictions.Add(predictionBooking);

            ViewBag.Predictions = TotalPredictions;
            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prediction predictionBooking)
        {
            ViewBag.BetPlatFormAndCode = "active";
            //ViewBag.BetPlatforms = await _betPlatformService.GetAllQueryable();
            //if (ModelState.IsValid)
            //{
            //    BookingPredictions.Add(predictionBooking);
            //}
            return View();
        }

        // GET: PredictionBookings/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var predictionBooking = await _context.PredictionBookings.FindAsync(id);
            //if (predictionBooking == null)
            //{
            //    return NotFound();
            //}
            return View();
        }

        // POST: PredictionBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PredictionBooking predictionBooking)
        {
            //if (id != predictionBooking.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(predictionBooking);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!PredictionBookingExists(predictionBooking.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            return View(predictionBooking);
        }

        // GET: PredictionBookings/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var predictionBooking = await _context.PredictionBookings
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (predictionBooking == null)
            //{
            //    return NotFound();
            //}

            return View();
        }

        // POST: PredictionBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            //var predictionBooking = await _context.PredictionBookings.FindAsync(id);
            //_context.PredictionBookings.Remove(predictionBooking);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PredictionBookingExists(long id)
        {
            return await _context.GetBookingById(id) != null ? true : false;
        }

        private  async Task AddPredictionToBooking(long id)
        {
            var predictionBooking = await _predictionService.GetById(id);
            BookingPredictionsStatic.Add(predictionBooking);
            TotalPredictions.Remove(predictionBooking);
            // TotalPredictions.Select(x => BookingPredictionsStatic.Any(y => x.Id != y.Id));

            ViewBag.Predictions = TotalPredictions;
        } 
        private  async Task RemovePredictionFromBooking(long id)
        {
            var predictionBooking = await _predictionService.GetById(id);
            BookingPredictionsStatic.Remove(predictionBooking);

            TotalPredictions.Add(predictionBooking);

            ViewBag.Predictions = TotalPredictions;
        }
    }
}
