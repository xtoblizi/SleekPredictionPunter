using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.BetPlatforms;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.GeneralUtilsAndServices;
using SleekPredictionPunter.Model.BettingPlatform;
using SleekPredictionPunter.WebApp.ViewModels.BetPlatforms;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class BetPlatformsController : Controller
    {
        private readonly IBetPlatformService _context;
        private readonly IFileHelper _fileHelper;

        public BetPlatformsController(IBetPlatformService context,IFileHelper fileHelper)
        {
            _context = context;
            _fileHelper = fileHelper;
        }

        // GET: BetPlanforms
        public async Task<IActionResult> Index(string search = null,int startindex=0, int count = int.MaxValue)
        {
             ViewBag.BetPlatFormAndCode = "active";

            Func<BetPlanform, DateTime> orderbyfunc = (x => x.DateCreated);
            search = string.IsNullOrWhiteSpace(search) ? string.Empty : search;
            Func<BetPlanform, bool> wherefunc = (x => x.BetPlatformName.Contains(search) || x.Caption.Contains(search));

            var result = await _context.GetAllQueryable(wherefunc, orderbyfunc, startindex, count);
            return View(result);
        }

        // GET: BetPlanforms/Details/5
        public async Task<IActionResult> Details(long id)
        {
            ViewBag.BetPlatFormAndCode = "atcive";

            var betPlanform = await _context.GetById(id);
            if (betPlanform == null)
            {
                return NotFound();
            }

            return View(betPlanform);
        }

        // GET: BetPlanforms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BetPlanforms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]BetPlatformVm betPlanformVm)
        {
            ViewBag.BetPlatFormAndCode = "atcive";

            if (ModelState.IsValid)
            {
                if(betPlanformVm.LogoPath != null)
                {

                    var createFileResult = await _fileHelper.SaveFileToCustomWebRootPath("files", betPlanformVm.LogoPath);
                    var betPlanform = new BetPlanform
                    {
                        BetPlatformName = betPlanformVm.BetPlatformName,
                        Caption = betPlanformVm.Caption,
                        LogoPath = createFileResult.CreatedFileRelativePath
                    };
                    await _context.Insert(betPlanform);
                    return RedirectToAction(nameof(Index));
                }
            }


            ViewBag.BetPlatforms = await _context.GetAllQueryable(null, (x => x.DateCreated), 0, 10);
            return View(betPlanformVm);
        }

        // GET: BetPlanforms/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var betPlanform = await _context.GetById(id);
            if (betPlanform == null)
            {
                return NotFound();
            }
            return View(betPlanform);
        }

        // POST: BetPlanforms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BetPlanform betPlanform)
        {
            ViewBag.BetPlatFormAndCode = "atcive";

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Update(betPlanform);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _context.GetById(betPlanform.Id) == null)
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
            return View(betPlanform);
        }

        // GET: BetPlanforms/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            ViewBag.BetPlatFormAndCode = "atcive";

            var betPlanform = await _context.GetById(id);
            if (betPlanform == null)
            {
                return NotFound();
            }

            return View(betPlanform);
        }

        // POST: BetPlanforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var model = await _context.GetById(id);

            await _context.Delete(model);

            return RedirectToAction(nameof(Index));
        }

      
    }
}
