using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.BetCategories;
using SleekPredictionPunter.AppService.CustomCategory;
using SleekPredictionPunter.AppService.MatchCategories;
using SleekPredictionPunter.AppService.PredictionCategoryService;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Categoriess;
using SleekPredictionPunter.Model.Enums;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class PredictionCategoriesController : Controller
    {
        private readonly ICategoryService _context;
        private readonly IMatchCategoryService _matchCategoryService;
        private readonly ICustomCategoryService _customCategoryService;
        private readonly IBetCategoryService _betCategoryService;

        public PredictionCategoriesController(
            ICategoryService context, 
            IBetCategoryService betCategoryService,
            IMatchCategoryService matchCategoryService,
            ICustomCategoryService customCategoryService)
        {
            _context = context;
            _betCategoryService = betCategoryService;
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
        
        /// <summary>
        /// Use this delete action method to delete all categories
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewToReturnTo"></param>
        /// <param name="categoryEnum"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteDynamic(long id,string viewToReturnTo,CategoriesType categoryEnum)
        {
            ViewBag.PredictionCategories = "active";
            if (categoryEnum == CategoriesType.BetCategory)
            {
                var model = await _betCategoryService.GetById(id);
                if (model != null)
                {
                    await _context.DeleteForAllCategories(model);
                    TempData["TempMessage"] = "Successfully Deleted The Category";
                }

            }
            else if (categoryEnum == CategoriesType.OddCategory) 
            {
                var model = await _context.GetCategoryById(id);
                if (model != null)
                {
                    await _context.DeleteForAllCategories(model);
                    TempData["TempMessage"] = "Successfully Deleted The Category";
                }
            }
            else if (categoryEnum == CategoriesType.SportCategory) 
            {
                var model = await _customCategoryService.GetById(id);
                if (model != null)
                {
                    await _context.DeleteForAllCategories(model);
                    TempData["TempMessage"] = "Successfully Deleted The Category";
                }
            }
            else if (categoryEnum == CategoriesType.MatchCategory) 
            {
                var model = await _matchCategoryService.GetById(id);
                if (model != null)
                {
                    await _context.DeleteForAllCategories(model);
                    TempData["TempMessage"] = "Successfully Deleted The Category";
                }
            }

            return RedirectToAction(viewToReturnTo);
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

        /// <summary>
        /// Create Match Categories such as  Laliga , EPL etc
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Create categorizations for the game /sports
        /// </summary>
        /// <returns></returns>
        #region custom/Sport Category catgory
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


        /// <summary>
        /// Create Bet Categories such as 2singles , 3sure Odds etc
        /// </summary>
        /// <returns></returns>
       #region BetCategory
        public async Task<IActionResult> CreateBetCategory()
        {
            ViewBag.PredictionCategories = "active";
            var getbetcategories = await _betCategoryService.GetAllQueryable(null,(x=>x.DateCreated),0,100);
            ViewBag.BetCategories = getbetcategories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBetCategory(BetCategory model)
        {
            ViewBag.BetCategories = "active";

            if (!ModelState.IsValid)
            {
                TempData["TempMessage"] = "Correctly fill all form fields";
                return RedirectToAction("CreateBetCategory");
            }
            await _betCategoryService.Insert(model);
            
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("CreateBetCategory");
        }
      
        #endregion

        private bool PredictionCategoryExists(long id)
        {
            var returnValue = _context.GetCategories();
            return returnValue.Result.Any(e => e.Id == id);
        }
    }

    
}
