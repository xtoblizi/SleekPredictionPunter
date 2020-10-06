using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SleekPredictionPunter.AppService.BetPlatforms;
using SleekPredictionPunter.AppService.BookingCodes;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.Model;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class BookingCodesController : Controller
    {
        private readonly IBookingCodeService _context;
        private readonly IBetPlatformService _betPlatformService;
        private readonly IPricingPlanAppService _pricingPlanAppService;

        public BookingCodesController(IBookingCodeService context,
            IBetPlatformService betPlatformService,IPricingPlanAppService pricingPlanAppService)
        {
            _context = context;
            _betPlatformService = betPlatformService;
            _pricingPlanAppService = pricingPlanAppService;
            
        }

        // GET: BookingCodes
        public async Task<IActionResult> Index(string search = null, int page = 0, int count = 20)
        {
            ViewBag.BookingCode = "active";
            ViewBag.BetPlatFormAndCode = "atcive";

            search = string.IsNullOrEmpty(search) ? string.Empty : search;

            Func<BookingCode, bool> wherefunc = (x => x.BetCode.Contains(search) || x.Betplatform.Contains(search));
            var skip = count * (page - 1);
            var result = await _context.GetAllQueryable(wherefunc: wherefunc, orderByFunc: (x=>x.DateCreated), startIndex : skip,count:count);

            var dashboardViewModel = new PaginationModel<BookingCode>
            {
                PerPage = count,
                CurrentPage = page,
                TotalRecordCountOfTheTable = await _context.GetCount(),
                TModel = result,
            };

            return View(dashboardViewModel);
        }

        // GET: BookingCodes/Details/5
        public async Task<IActionResult> Details(long id)
        {
            ViewBag.BetPlatFormAndCode = "atcive";
            var bookingCode = await _context.GetById(id);
            if (bookingCode == null)
            {
                return NotFound();
            }

            return View(bookingCode);
        }

        // GET: BookingCodes/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.BookingCode = "active";
            ViewBag.BetPlatFormAndCode = "atcive";

            ViewBag.PackageId = new SelectList(await _pricingPlanAppService.GetAllPlans(), "Id", "PlanName");
            ViewBag.BetPlatformId = new SelectList(await _betPlatformService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "BetPlatformName");
           
            return View();
        }

        // POST: BookingCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCode bookingCode)
        {
            ViewBag.BookingCode = "active";
            ViewBag.BetPlatFormAndCode = "atcive";

            if (ModelState.IsValid)
            {
                var package = await _pricingPlanAppService.GetById(bookingCode.PricingPlanId);
                var betplatform = await _betPlatformService.GetById(bookingCode.BetPlatformId);

                bookingCode.Betplatform = betplatform.BetPlatformName;
                bookingCode.PricingPlan = package.PlanName;

                await _context.Insert(bookingCode);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PackageId = new SelectList(await _pricingPlanAppService.GetAllPlans(), "Id", "PlanName");
            ViewBag.BetPlatformId = new SelectList(await _betPlatformService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "Name");

            return View(bookingCode);
        }

        // GET: BookingCodes/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            ViewBag.BookingCode = "active";
            ViewBag.BetPlatFormAndCode = "atcive";

            var bookingCode = await _context.GetById(id);
            if (bookingCode == null)
            {
                return NotFound();
            }
            ViewBag.PackageId = new SelectList(await _pricingPlanAppService.GetAllPlans(), "Id", "PlanName");
            ViewBag.BetPlatformId = new SelectList(await _betPlatformService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "Name");

            return View(bookingCode);
        }

        // POST: BookingCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookingCode bookingCode)
        {
            if ( await _context.GetById(bookingCode.Id) == null)
                ViewBag.BetPlatFormAndCode = "atcive";
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PackageId = new SelectList(await _pricingPlanAppService.GetAllPlans(), "Id", "PlanName");
            ViewBag.BetPlatformId = new SelectList(await _betPlatformService.GetAllQueryable(null, (x => x.DateCreated), 0, 100), "Id", "Name");
            return View(bookingCode);
        }

        // GET: BookingCodes/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            ViewBag.BookingCode = "active";
            ViewBag.BetPlatFormAndCode = "atcive";
            var bookingCode = await _context.GetById(id);
            if (bookingCode == null)
            {
                return NotFound();
            }

            return View(bookingCode);
        }

        // POST: BookingCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var bookingCode = await _context.GetById(id);
            await _context.Delete(bookingCode);
            return RedirectToAction(nameof(Index));
        }

    }
}
