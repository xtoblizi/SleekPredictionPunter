using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.Withdrawals;
using SleekPredictionPunter.Model;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class WithdrawalsController : Controller
    {
        private readonly IWithdrawalService _context;

        public WithdrawalsController(IWithdrawalService context)
        {
            _context = context;
        }

        // GET: Withdrawals
        public async Task<IActionResult> Index(int page = 1,int count = 50)
        {

            var result = await _context.GetWithdrawals(null, (x => x.DateCreated), startIndex: page, take: count);
            return View(result);
        }

        // GET: Withdrawals/Details/5
        public async Task<IActionResult> Details(long id)
        {           
            var withdrawal = await _context.GetById(id);
            if (withdrawal == null)
                return NotFound();
            
            return View(withdrawal);
        }

        // GET: Withdrawals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Withdrawals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Withdrawal withdrawal)
        {
            if (ModelState.IsValid)
            {
                await _context.Insert(withdrawal);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Withdrawals/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            //var model = await _context.GetById(id);

            //await _context.Update(model);
           
            return View();
        }

        // POST: Withdrawals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Withdrawal model)
        {
            if (ModelState.IsValid)
            {
                await _context.Update(model);
                return RedirectToAction(nameof(Index));
            }
                
            return View();
        }

        // GET: Withdrawals/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var model = await _context.GetById(id);

            return View(model);
        } 

        public async Task<IActionResult> Delete(Withdrawal model)
        {
            await _context.Delete(model);
            ViewBag.Message = "Delete Succesful";

            return View();
        }
    }
}
