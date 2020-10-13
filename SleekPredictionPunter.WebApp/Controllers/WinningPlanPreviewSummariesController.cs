using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SleekPredictionPunter.AppService.HomeWiningPlanPreview;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.Model.HomeDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{

    public class WinningPlanPreviewSummariesController : BaseController
    {
        private readonly IWiningPlanPreviewService _context;
        private readonly IPricingPlanAppService _pricingPlanAppService;

        public WinningPlanPreviewSummariesController(IWiningPlanPreviewService context, 
            IPricingPlanAppService pricingPlanAppService)
        {
            _context = context;
            _pricingPlanAppService = pricingPlanAppService;
        }

        // GET: WinningPlanPreviewSummaries
        public async Task<IActionResult> Index()
        {
            ViewBag.WinningPlanPreviewSummaries = "active";
            ViewBag.ResultandAdverts = "active";

            Func<WinningPlanPreviewSummary, DateTime> ordebyfunc = (x => x.DateCreated);
            var result = await _context.GetAllQueryable(null, orderByFunc: ordebyfunc, startIndex: 0, count: 100);
            ViewBag.HasValue = result.Any() ? true : false;
            return View(result);
        }

        // GET: WinningPlanPreviewSummaries/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var winningPlanPreviewSummary = await _context.GetById(id.Value);
            if (winningPlanPreviewSummary == null)
            {
                return NotFound();
            }

            return View(winningPlanPreviewSummary);
        }

        // GET: WinningPlanPreviewSummaries/Create
        public async Task<IActionResult> Create()
        {
            var ratings = new List<RatingValue>
            {
                new RatingValue{Id= 1, RateName = "1 Star"},
                new RatingValue{Id= 2, RateName = "2 Star"},
                new RatingValue{Id= 3, RateName = "3 Star"},
                new RatingValue{Id= 4, RateName = "4 Star"},
                new RatingValue{Id= 5, RateName = "5 Star"}
            };

            var plans = await _pricingPlanAppService.GetAllPlans();

            ViewBag.RatingId = new SelectList(ratings, "Id", "RateName");
            ViewBag.PricingPlanId = new SelectList(plans, "Id", "PlanName");
            ViewBag.ResultandAdverts = "active";

            return View();
        }

        // POST: WinningPlanPreviewSummaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WinningPlanPreviewSummary winningPlanPreviewSummary)
        {
            if (ModelState.IsValid)
            {
               
                var plan = await _pricingPlanAppService.GetById(winningPlanPreviewSummary.PricingPlanId);
                winningPlanPreviewSummary.PlanName = plan.PlanName;
                await _context.Insert(winningPlanPreviewSummary);

                return RedirectToAction(nameof(Index));
            }

            var ratings = new List<RatingValue>
            {
                new RatingValue{Id= 1, RateName = "1 Star"},
                new RatingValue{Id= 2, RateName = "2 Star"},
                new RatingValue{Id= 3, RateName = "3 Star"},
                new RatingValue{Id= 4, RateName = "4 Star"},
                new RatingValue{Id= 5, RateName = "5 Star"}
            };

            var plans = await _pricingPlanAppService.GetAllPlans();
            ViewBag.PricingPlanId = new SelectList(plans, "Id", "PlanName", winningPlanPreviewSummary.PricingPlanId);
            ViewBag.RatingId = new SelectList(ratings, "Id", "RateName");
            ViewBag.ResultandAdverts = "active";


            return View(winningPlanPreviewSummary);
        }

        // GET: WinningPlanPreviewSummaries/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            var result = await _context.GetById(id.Value);
            if (result != null)
            {
                var ratings = new List<RatingValue>
                {
                    new RatingValue{Id= 1, RateName = "1 Star"},
                    new RatingValue{Id= 2, RateName = "2 Star"},
                    new RatingValue{Id= 3, RateName = "3 Star"},
                    new RatingValue{Id= 4, RateName = "4 Star"},
                    new RatingValue{Id= 5, RateName = "5 Star"}
                };

                var plans = await _pricingPlanAppService.GetAllPlans();
                ViewBag.PricingPlanId = new SelectList(plans, "Id", "PlanName");
                ViewBag.RatingId = new SelectList(ratings, "Id", "RateName");
                ViewBag.ResultandAdverts = "active";

                return View(result);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: WinningPlanPreviewSummaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WinningPlanPreviewSummary winningPlanPreviewSummary)
        {
            var plan = await _pricingPlanAppService.GetById(winningPlanPreviewSummary.PricingPlanId);
            if (ModelState.IsValid)
            {
                var model = await _context.GetById(winningPlanPreviewSummary.Id);
                model.PricingPlanId = winningPlanPreviewSummary.PricingPlanId;
                model.RatingValue = winningPlanPreviewSummary.RatingValue;
                model.PlanName = plan.PlanName;
                model.Paragraph1Description = winningPlanPreviewSummary.Paragraph1Description;
                model.Paragraph2Description = winningPlanPreviewSummary.Paragraph2Description;
                model.Paragraph3Description = winningPlanPreviewSummary.Paragraph3Description;
                model.AdvertActionMessage = winningPlanPreviewSummary.AdvertActionMessage;
                model.SetforHomePreview = winningPlanPreviewSummary.SetforHomePreview;

                await _context.Update(model);
                                
                return RedirectToAction(nameof(Index));
            }
            var ratings = new List<RatingValue>
            {
                new RatingValue{Id= 1, RateName = "1 Star"},
                new RatingValue{Id= 2, RateName = "2 Star"},
                new RatingValue{Id= 3, RateName = "3 Star"},
                new RatingValue{Id= 4, RateName = "4 Star"},
                new RatingValue{Id= 5, RateName = "5 Star"}
            };

            var plans = await _pricingPlanAppService.GetAllPlans();
            ViewBag.PricingPlanId = new SelectList(plans, "Id", "PlanName", winningPlanPreviewSummary.PricingPlanId);
            ViewBag.RatingId = new SelectList(ratings, "Id", "RateName");
            ViewBag.ResultandAdverts = "active";

            return View(winningPlanPreviewSummary);
        }

        // GET: WinningPlanPreviewSummaries/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var winningPlanPreviewSummary = await _context.GetById(id.Value);
            if (winningPlanPreviewSummary == null)
            {
                return NotFound();
            }
            return View(winningPlanPreviewSummary);
        }

        // POST: WinningPlanPreviewSummaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var winningPlanPreviewSummary = await _context.GetById(id);
            
            await _context.Delete(winningPlanPreviewSummary);
          
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> WinningPlanPreviewSummaryExists(long id)
        {
            var result = await _context.GetById(id);
            return result == null ? false : true;
        }
    }
}
