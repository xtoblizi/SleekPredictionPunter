//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using SleekPredictionPunter.DataInfrastructure;
//using SleekPredictionPunter.Model;

//namespace SleekPredictionPunter.WebApp.Controllers
//{
//    public class Predictions1Controller : Controller
//    {
//        private readonly PredictionDbContext _context;

//        public Predictions1Controller(PredictionDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Predictions1
//        public async Task<IActionResult> Index()
//        {
//            var predictionDbContext = _context.Predictions.Include(p => p.CustomCategory).Include(p => p.MatchCategory).Include(p => p.PredictionCategory).Include(p => p.Predictor).Include(p => p.PricingPlan);
//            return View(await predictionDbContext.ToListAsync());
//        }

//        // GET: Predictions1/Details/5
//        public async Task<IActionResult> Details(long? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var prediction = await _context.Predictions
//                .Include(p => p.CustomCategory)
//                .Include(p => p.MatchCategory)
//                .Include(p => p.PredictionCategory)
//                .Include(p => p.Predictor)
//                .Include(p => p.PricingPlan)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (prediction == null)
//            {
//                return NotFound();
//            }

//            return View(prediction);
//        }

//        // GET: Predictions1/Create
//        public IActionResult Create()
//        {
//            ViewData["CustomCategoryId"] = new SelectList(_context.CustomCategories, "Id", "Id");
//            ViewData["MatchCategoryId"] = new SelectList(_context.MatchCategories, "Id", "Id");
//            ViewData["PredictionCategoryId"] = new SelectList(_context.PredictionCategories, "Id", "Id");
//            ViewData["PredictorId"] = new SelectList(_context.Predictors, "Id", "Id");
//            ViewData["PricingPlanId"] = new SelectList(_context.PricePlans, "Id", "Id");
//            return View();
//        }

//        // POST: Predictions1/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("PredictorUserName,ClubA,ClubAOdd,ClubALogoPath,ClubB,ClubBOdd,ClubBLogoPath,PredictionValue,TimeofFixture,PredictorId,CustomCategoryId,MatchCategoryId,PredictionCategoryId,PricingPlanId,ClubAScore,ClubBScore,Id,DateCreated,EntityStatus,DateUpdated")] Prediction prediction)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(prediction);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CustomCategoryId"] = new SelectList(_context.CustomCategories, "Id", "Id", prediction.CustomCategoryId);
//            ViewData["MatchCategoryId"] = new SelectList(_context.MatchCategories, "Id", "Id", prediction.MatchCategoryId);
//            ViewData["PredictionCategoryId"] = new SelectList(_context.PredictionCategories, "Id", "Id", prediction.PredictionCategoryId);
//            ViewData["PredictorId"] = new SelectList(_context.Predictors, "Id", "Id", prediction.PredictorId);
//            ViewData["PricingPlanId"] = new SelectList(_context.PricePlans, "Id", "Id", prediction.PricingPlanId);
//            return View(prediction);
//        }

//        // GET: Predictions1/Edit/5
//        public async Task<IActionResult> Edit(long? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var prediction = await _context.Predictions.FindAsync(id);
//            if (prediction == null)
//            {
//                return NotFound();
//            }
//            ViewData["CustomCategoryId"] = new SelectList(_context.CustomCategories, "Id", "Id", prediction.CustomCategoryId);
//            ViewData["MatchCategoryId"] = new SelectList(_context.MatchCategories, "Id", "Id", prediction.MatchCategoryId);
//            ViewData["PredictionCategoryId"] = new SelectList(_context.PredictionCategories, "Id", "Id", prediction.PredictionCategoryId);
//            ViewData["PredictorId"] = new SelectList(_context.Predictors, "Id", "Id", prediction.PredictorId);
//            ViewData["PricingPlanId"] = new SelectList(_context.PricePlans, "Id", "Id", prediction.PricingPlanId);
//            return View(prediction);
//        }

//        // POST: Predictions1/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(long id, [Bind("PredictorUserName,ClubA,ClubAOdd,ClubALogoPath,ClubB,ClubBOdd,ClubBLogoPath,PredictionValue,TimeofFixture,PredictorId,CustomCategoryId,MatchCategoryId,PredictionCategoryId,PricingPlanId,ClubAScore,ClubBScore,Id,DateCreated,EntityStatus,DateUpdated")] Prediction prediction)
//        {
//            if (id != prediction.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(prediction);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!PredictionExists(prediction.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CustomCategoryId"] = new SelectList(_context.CustomCategories, "Id", "Id", prediction.CustomCategoryId);
//            ViewData["MatchCategoryId"] = new SelectList(_context.MatchCategories, "Id", "Id", prediction.MatchCategoryId);
//            ViewData["PredictionCategoryId"] = new SelectList(_context.PredictionCategories, "Id", "Id", prediction.PredictionCategoryId);
//            ViewData["PredictorId"] = new SelectList(_context.Predictors, "Id", "Id", prediction.PredictorId);
//            ViewData["PricingPlanId"] = new SelectList(_context.PricePlans, "Id", "Id", prediction.PricingPlanId);
//            return View(prediction);
//        }

//        // GET: Predictions1/Delete/5
//        public async Task<IActionResult> Delete(long? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var prediction = await _context.Predictions
//                .Include(p => p.CustomCategory)
//                .Include(p => p.MatchCategory)
//                .Include(p => p.PredictionCategory)
//                .Include(p => p.Predictor)
//                .Include(p => p.PricingPlan)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (prediction == null)
//            {
//                return NotFound();
//            }

//            return View(prediction);
//        }

//        // POST: Predictions1/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(long id)
//        {
//            var prediction = await _context.Predictions.FindAsync(id);
//            _context.Predictions.Remove(prediction);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool PredictionExists(long id)
//        {
//            return _context.Predictions.Any(e => e.Id == id);
//        }
//    }
//}
