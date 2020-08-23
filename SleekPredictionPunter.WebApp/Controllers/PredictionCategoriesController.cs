using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.CustomCategory;
using SleekPredictionPunter.AppService.MatchCategories;
using SleekPredictionPunter.AppService.PredictionCategoryService;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class PredictionCategoriesController : Controller
    {
        private readonly ICategoryService _context;
        private readonly IMatchCategoryService _matchCategoryService;
        private readonly ICustomCategoryService _customCategoryService;

        public PredictionCategoriesController(
            ICategoryService context, 
            IMatchCategoryService matchCategoryService,
            ICustomCategoryService customCategoryService)
        {
            _context = context;
            _matchCategoryService = matchCategoryService;
            _customCategoryService = customCategoryService;
        }

        #region prediction category
        // GET: PredictionCategories
        public async Task<IActionResult> Index()
        {
            ViewBag.PredictionCategories = "active";
            return View(await _context.GetCategories());
        }

        // GET: PredictionCategories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            ViewBag.PredictionCategories = "active";
            if (id == null)
            {
                return NotFound();
            }

            var predictionCategory = await _context.GetCategoryById(id.Value);
            if (predictionCategory == null)
            {
                return NotFound();
            }

            return View(predictionCategory);
        }

        // GET: PredictionCategories/Create
        public IActionResult Create()
        {
            ViewBag.PredictionCategories = "active";
            return View();
        }

        // POST: PredictionCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PredictionCategory predictionCategory)
        {
            ViewBag.PredictionCategories = "active";
            predictionCategory.EntityStatus = EntityStatusEnum.Active;
            predictionCategory.DateCreated = DateTime.UtcNow;
            predictionCategory.DateUpdated = DateTime.UtcNow;
            predictionCategory.CreatorUserName = User.Identity.Name;
            if (ModelState.IsValid)
            { 
                await _context.CreateCategory(predictionCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(predictionCategory);
        }
         
        // GET: PredictionCategories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            ViewBag.PredictionCategories = "active";
            if (id == null)
            {
                return NotFound();
            }

            var predictionCategory = await _context.GetCategoryById(id.Value);
            if (predictionCategory == null)
            {
                return NotFound();
            }

            return View(predictionCategory);
        }

        // POST: PredictionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var predictionCategory = await _context.GetCategoryById(id);
            await _context.RemoveCategoryBy(predictionCategory);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region match catgory
        public async Task<IActionResult> CreateMatchCategory()
        {
            ViewBag.PredictionCategories = "active";
            var getMatchCategory =  await _matchCategoryService.GetAllQueryable();
            ViewBag.MatchCategory = getMatchCategory.OrderByDescending(x => x.Id);
            return View();
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMatchCategory(MatchCategory model)
        {
            ViewBag.PredictionCategories = "active";  
            if (ModelState.IsValid)
            {
                await _matchCategoryService.Insert(model);
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("CreateMatchCategory");
            }
            return View(model);
        }

        #endregion

        #region custom catgory
        public async Task<IActionResult> CreateCustomCategory()
        {
            ViewBag.PredictionCategories = "active";
            var getCustomCategory = await _customCategoryService.GetAllQueryable();
            ViewBag.CustomCategory = getCustomCategory.OrderByDescending(x => x.Id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomCategory(CustomCategory model)
        {
            ViewBag.PredictionCategories = "active";
            if (ModelState.IsValid)
            {
                await _customCategoryService.Insert(model);
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("CreateCustomCategory");
            }
            return View(model);
        }

        #endregion

        private bool PredictionCategoryExists(long id)
        {
            var returnValue = _context.GetCategories();
            return returnValue.Result.Any(e => e.Id == id);
        }
    }
}
