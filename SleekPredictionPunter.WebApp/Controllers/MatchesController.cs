using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.AppService.Matches;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.Model.Matches;
using System;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly IPredictionService _predictionService;

        public MatchesController(IMatchService matchService, IPredictionService predictionService)
        {
            _matchService = matchService;
            _predictionService = predictionService;
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            return View(await _matchService.GetMatches());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClubA,ClubALogoPath,ClubB,ClubBLogoPath,MatchCategory,MatchCategoryId,SportCategory,SportCategoryId,TimeofMatch,MatchStatus,IsSetAsHotPreview,Id,DateCreated,EntityStatus,DateUpdated")] Match match)
        {
            if (ModelState.IsValid)
            {
                await _matchService.Insert(model:match, savechanges: true);
                return RedirectToAction(nameof(Index));
            }
            return View(match);
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
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ClubA,ClubALogoPath,ClubB,ClubBLogoPath,MatchCategory,MatchCategoryId,SportCategory,SportCategoryId,TimeofMatch,MatchStatus,IsSetAsHotPreview,Id,DateCreated,EntityStatus,DateUpdated")] Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _matchService.Update(match);
                   
                }
                catch (Exception)
                {
                    if (!await MatchExists(match.Id))
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
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await  _matchService.GetMatchById(id.Value);
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
