using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
using SleekPredictionPunter.Model.Matches;
using System;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IPredictionService _predictionService;
        private readonly IMatchService _matchService;
        private readonly IClubService _clubService;
        private readonly ICategoryService _categoryService;
        private readonly IMatchCategoryService _matchCategoryService;
        private readonly ICustomCategoryService _customCategoryService;

        public MatchesController(IMatchService matchService,
            IClubService clubService,
            ICategoryService categoryService,
            IMatchCategoryService matchCategoryService,
            ICustomCategoryService customCategoryService,
            IPredictionService predictionService)
        {
            _matchService = matchService;
            _clubService = clubService;
            _categoryService = categoryService;
            _predictionService = predictionService;
            _matchCategoryService = matchCategoryService;
            _customCategoryService = customCategoryService;
        }

        // GET: Matches
        public async Task<IActionResult> Index(int startIndex = 0,int count = 200)
        {
            return View(await _matchService.GetMatches<DateTime>(null,(x=>x.DateCreated),startIndex,count));
        }
        public async Task<IActionResult> _FrontEndPartialView()
        {
            Func<Match, bool> wherefunc = (x => x.MatchStatus == MatchStatusEnum.Upcoming || x.MatchStatus == MatchStatusEnum.Playing);
            Func<Match, DateTime> orderByDescFun = (x => x.DateCreated);
            var result = await _matchService.GetMatches(whereFunc: null, orderByDescFunc: orderByDescFun, startIndex: 0, count: 4);
            return View(result);
        }
        public async Task<IActionResult> FrontEndIndex()
        {
            return View(await _matchService.GetMatches());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchService.GetMatchById(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Predictions = "active";

            ViewBag.ClubAId = new SelectList(await _clubService.GetAllQueryable(), "Id", "ClubName");
            ViewBag.ClubBId = new SelectList(await _clubService.GetAllQueryable(), "Id", "ClubName");
            ViewBag.PredictionCategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "GetNameAndDescription");
            ViewBag.MatchCategoryId = new SelectList(await _matchCategoryService.GetAllQueryable(), "Id", "CategoryName");
            ViewBag.CustomCategoryId = new SelectList(await _customCategoryService.GetAllQueryable(), "Id", "CategoryName");

            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ClubA,ClubALogoPath,ClubB,ClubBLogoPath,MatchCategory,MatchCategoryId," +
        //    "CustomCategory,CustomCategoryId,TimeofMatch,MatchStatus,IsSetAsHotPreview,Id,DateCreated," +
        //    "EntityStatus,DateUpdated")] Match match)   
        public async Task<IActionResult> Create(Match match)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Predictions = "active";

                ViewBag.ClubAId = new SelectList(await _clubService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "ClubName");
                ViewBag.ClubBId = new SelectList(await _clubService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "ClubName");
                ViewBag.MatchCategoryId = new SelectList(await _matchCategoryService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "CategoryName");
                ViewBag.CustomCategoryId = new SelectList(await _customCategoryService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "CategoryName");

                return View();
            }
            
            var clubA = await _clubService.GetById(match.ClubAId);
            var clubB = await _clubService.GetById(match.ClubBId);
            var sportCategory = await _customCategoryService.GetById(match.CustomCategoryId);
            var matchCategory = await _matchCategoryService.GetById(match.MatchCategoryId);

            match.ClubA = clubA.ClubName;
            match.ClubALogoPath = clubA.ClubLogRelativePath;
            match.ClubB = clubB.ClubName;
            match.ClubBLogoPath = clubB.ClubLogRelativePath;
            match.CustomCategory = sportCategory.CategoryName;
            match.MatchCategory = matchCategory.CategoryName;
            match.MatchStatus = MatchStatusEnum.Upcoming;
            
            await _matchService.Insert(model: match, savechanges: true);
            return RedirectToAction(nameof(Index));
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchService.GetMatchById(id.Value);
            if (match == null)
            {
                return NotFound();
            }
            var rolesEnumList = EnumHelper.GetEnumResults<MatchStatusEnum>();
            ViewBag.MatchStatus = new SelectList(rolesEnumList, "Id", "Name",(int)match.MatchStatus);

            ViewBag.ClubA = new SelectList(await _clubService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "ClubName", "ClubName");
            ViewBag.ClubB = new SelectList(await _clubService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "ClubName", "ClubName");
            ViewBag.PredictionCategoryId = new SelectList(await _categoryService.GetCategories(null, (x => x.DateCreated), 0, 100), "Id", "GetNameAndDescription");
            ViewBag.MatchCategoryId = new SelectList(await _matchCategoryService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "CategoryName", match.MatchCategoryId);
            ViewBag.CustomCategoryId = new SelectList(await _customCategoryService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "CategoryName", match.CustomCategoryId);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Match match)
        {
            //if (id != match.Id)
            //{
            //    return NotFound();
            //}

            if (match.TimeofMatch < DateTime.Now && (match.ReturnStatus== MatchStatusEnum.Past || match.ReturnStatus == MatchStatusEnum.Playing))
            {
                TempData["TempMessage"] = "The Time of the match and the status conflict";
                return View(match);
            }

            if (match.TimeofMatch < DateTime.Now.AddMinutes(-20))
            {
                TempData["TempMessage"] = "You can not update a match 20minutes before its kick-off-time";
                return View(match);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var model = await _matchService.GetMatchById(match.Id);

                    TempData["TempMessage"] = $"The Match Between {match.ClubA} vs {match.ClubB} Has Been Successfully Updated";
                    var matchCategory = await _matchCategoryService.GetById(match.MatchCategoryId);
                    var sportCat = await _customCategoryService.GetById(match.CustomCategoryId);

                    model.CustomCategory = sportCat.CategoryName;
                    model.CustomCategoryId = sportCat.Id;
                    model.MatchCategory = matchCategory.CategoryName;
                    model.MatchCategoryId = matchCategory.Id;
                    model.MatchStatus = (MatchStatusEnum)match.MatchStatus;
                    model.IsSetAsHotPreview = match.IsSetAsHotPreview;
                    model.TimeofMatch = match.TimeofMatch;


                    await _matchService.Update(model);

                    // get all predictions created on that match
                    //var predictions = await _predictionService.GetByMatchId(match.Id);

                    // loop through the predictions and update thier status and time.
                    //foreach (var item in predictions)
                    //{

                    //}

                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("ClubA,ClubALogoPath,ClubB,ClubBLogoPath,MatchCategory,MatchCategoryId,CustomCategory,CustomCategoryId,TimeofMatch,MatchStatus,IsSetAsHotPreview,Id,DateCreated,EntityStatus,DateUpdated")] Match match)
        //{
        //    if (id != match.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await _matchService.Update(match);

        //        }
        //        catch (Exception)
        //        {
        //            if (!await MatchExists(match.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(match);
        //}

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchService.GetMatchById(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var match = await _matchService.GetMatchById(id);
            await _matchService.Delete(match);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MatchExists(long id)
        {
            var result = await _matchService.GetMatchById(id);
            return result != null ? true : false;
        }
    }
}
