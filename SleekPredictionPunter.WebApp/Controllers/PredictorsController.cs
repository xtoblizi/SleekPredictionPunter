using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class PredictorsController : Controller
    {
        private readonly PredictionDbContext _context;
        private readonly IPredictorService _predictorService;
        public PredictorsController(PredictionDbContext context, IPredictorService predictorService)
        {
            _context = context;
            _predictorService = predictorService;
        }

        // GET: Predictors
        public async Task<IActionResult> Index()
        {
            return View(await _predictorService.GetAllQueryable());
        }

        // GET: Predictors/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predictor = await _predictorService.GetById(id.Value);
            if (predictor == null)
            {
                return NotFound();
            }

            return View(predictor);
        }

        // GET: Predictors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Predictors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Predictor predictor)
        {
            if (ModelState.IsValid)
            {
                await _predictorService.Insert(predictor);
                return RedirectToAction(nameof(Index));
            }
            return View(predictor);
        }

        // GET: Predictors/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predictor = await _predictorService.GetById(id.Value);
            if (predictor == null)
            {
                return NotFound();
            }
            return View(predictor);
        }

        // POST: Predictors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Predictor predictor)
        {
            if (id != predictor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _predictorService.Update(predictor, true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await PredictorExists(predictor.Id))
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
            return View(predictor);
        }

        // GET: Predictors/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predictor = await _predictorService.GetById(id.Value);
            if (predictor == null)
            {
                return NotFound();
            }

            return View(predictor);
        }

        // POST: Predictors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var predictor = await _predictorService.GetById(id);
            await _predictorService.Delete(predictor, true);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PredictorExists(long id)
        {
            var chechIfExist = await _predictorService.GetAllQueryable();
            return chechIfExist.Any(e => e.Id == id);
        }
    }
}
